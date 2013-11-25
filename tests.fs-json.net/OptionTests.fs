namespace Tests.Newtonsoft.Json.FSharp
open Newtonsoft.Json
open NUnit.Framework
open Newtonsoft.Json.FSharp
open FsUnit

module OptionTests = 

    type Widget = {
        Name: string option
        Age: int64 option
        Male: bool option
    }

    let converters : JsonConverter[] = [| TupleConverter()
                                          OptionConverter() |]

    let settings = 
      JsonSerializerSettings(
        PreserveReferencesHandling  = PreserveReferencesHandling.All, 
        Converters                  = converters)

    let toJSON v = 
      JsonConvert.SerializeObject(v,Formatting.Indented,converters)
    let ofJSON (v) : 't = 
      JsonConvert.DeserializeObject<'t>(v,converters)

    [<Test>]
    let ``Primative value maps to Some`` () : unit =
        let result : Widget = ofJSON "{ \"name\": \"Foo\", \"age\": 31, \"male\": true }"
        result.Name |> should equal (Some "Foo") 
        result.Age |> should equal (Some 31L) 
        result.Male |> should equal (Some true) 

    [<Test>]
    let ``Missing values map to none`` () : unit =
        let result : Widget = ofJSON "{ }"
        result.Name |> should equal None
        result.Age |> should equal None
        result.Male |> should equal None