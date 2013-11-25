﻿namespace Tests.Newtonsoft.Json.FSharp
open Newtonsoft.Json
open NUnit.Framework
open Newtonsoft.Json.FSharp
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
        let result : Widget = ofJSON "{ \"name\": \"Foo\", \"age\": 31, \"male\": true, \"parts\": { \"bigParts\": [], \"smallParts\": [ { \"name\": \"engine\" } ] } }"
        result.Name |> should equal (Some "Foo") 
        result.Age |> should equal (Some 31L) 
        result.Male |> should equal (Some true) 
        result.Parts |> should equal (Some { BigParts = List.empty; SmallParts = [ { Name = "engine" } ] }) 

    [<Test>]
    let ``Missing values map to none`` () : unit =
        let result : Widget = ofJSON "{ }"
        result.Name |> should equal None
        result.Age |> should equal None
        result.Male |> should equal None
        result.Parts |> should equal None