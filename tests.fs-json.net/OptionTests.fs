namespace Tests.Newtonsoft.Json.FSharp
open Newtonsoft.Json
open NUnit.Framework
open Newtonsoft.Json.FSharp
open System
open FsUnit

module OptionTests = 
    
    type Part = {
        Name: string
    }

    type PartSet = {
        BigParts: Part list
        SmallParts: Part list
    }

    type Widget = {
        Name: string option
        Age: int64 option
        Male: bool option
        DateOfBirth: DateTime option
        Parts: PartSet option
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
    let ``Can round trip option values`` () : unit =
        let value = {
                            Name = Some "Foo"
                            Age = Some 31L
                            Male = Some true
                            DateOfBirth = Some (DateTime.Parse("2013-11-29T00:51:00Z"))
                            Parts = Some 
                                {
                                    BigParts = []
                                    SmallParts = [{ Name = "engine"}]
                                }
                           }
        let stringResult = toJSON value
        let result : Widget = ofJSON stringResult
        result |> should equal value

    [<Test>]
    let ``Primative value maps to Some`` () : unit =
        let result : Widget = ofJSON "{ \"name\": \"Foo\", \"age\": 31, \"dateOfBirth\": \"2013-11-29T00:51:00Z\", \"male\": true, \"parts\": { \"bigParts\": [], \"smallParts\": [ { \"name\": \"engine\" } ] } }"
        result.Name |> should equal (Some "Foo") 
        result.Age |> should equal (Some 31L) 
        result.DateOfBirth |> should equal (Some (new DateTime(2013, 11, 29,00, 51, 0, 0, DateTimeKind.Utc))) 
        result.Male |> should equal (Some true) 
        result.Parts |> should equal (Some { BigParts = List.empty; SmallParts = [ { Name = "engine" } ] }) 

    [<Test>]
    let ``Missing values map to none`` () : unit =
        let result : Widget = ofJSON "{ }"
        result.Name |> should equal None
        result.Age |> should equal None
        result.Male |> should equal None
        result.Parts |> should equal None

    [<Test>]
    let ``Null values map to none`` () : unit =
        let result : Widget = ofJSON "{ \"name\": null, \"age\": null, \"male\": null, \"parts\": null, \"dateOfBirth\": null }"
        result.Name |> should equal None
        result.Age |> should equal None
        result.Male |> should equal None
        result.Parts |> should equal None
        result.DateOfBirth |> should equal None

    type DateContainer = {
        DateOfBirth: DateTime option
    }

    [<Test>]
    let ``Null option<DateTime> maps to None`` () : unit =
        let result : Widget = ofJSON "{ \"dateOfBirth\": null }"
        match result.DateOfBirth with
        | Some x -> Assert.Fail("Should have been None")
        | None -> Assert.Pass()