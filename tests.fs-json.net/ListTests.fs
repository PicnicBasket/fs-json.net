namespace Tests.Newtonsoft.Json.FSharp
open Newtonsoft.Json
open NUnit.Framework
open Newtonsoft.Json.FSharp
open FsUnit

module ListTests = 
    type Part = {
        Name: string
    }

    type PartSet = {
        BigParts: Part list
        SmallParts: Part list
    }

    type Widget = {
        Parts: Part list
        PartSet: PartSet
    }

    let converters : JsonConverter[] = [| TupleConverter()
                                          OptionConverter()
                                          ListConverter() |]

    let settings = 
      JsonSerializerSettings(
        PreserveReferencesHandling  = PreserveReferencesHandling.All, 
        Converters                  = converters)

    let toJSON v = 
      JsonConvert.SerializeObject(v,Formatting.Indented,converters)
    let ofJSON (v) : 't = 
      JsonConvert.DeserializeObject<'t>(v,converters)

    [<Test>]
    let ``Can deserialize list`` () : unit =
        let result : Widget = ofJSON "{ \"parts\": [ { \"name\": \"engine\" } ], \"partSet\": { \"bigParts\": [], \"smallParts\": [ { \"name\": \"engine\" } ] }}"
        result.Parts |> should equal ([ { Name = "engine" } ]) 
        result.PartSet |> should equal ({ BigParts = List.empty; SmallParts = [ { Name = "engine" } ] }) 