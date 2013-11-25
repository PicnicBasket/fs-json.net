namespace Tests.Newtonsoft.Json.FSharp
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
    let ``Can encode option values`` () : unit =
        let stringResult = toJSON {
                            Name = Some "Foo"
                            Age = Some 31L
                            Male = Some true
                            Parts = Some 
                                {
                                    BigParts = []
                                    SmallParts = [{ Name = "engine"}]
                                }
                           }
        stringResult |> ignore

    [<Test>]
    let ``Can decode values that have Value property`` () : unit =
        let sw = new System.IO.StringWriter()
        fprintfn sw "{"
        fprintfn sw "\"Name\": {"
        fprintfn sw "\"Value\": \"Foo\""
        fprintfn sw "},"
        fprintfn sw "\"Age\": {"
        fprintfn sw "\"Value\": 31"
        fprintfn sw "},"
        fprintfn sw " \"Male\": {"
        fprintfn sw "\"Value\": true"
        fprintfn sw " },"
        fprintfn sw "\"Parts\": {"
        fprintfn sw "\"Value\": {"
        fprintfn sw "\"BigParts\": [],"
        fprintfn sw "\"SmallParts\": ["
        fprintfn sw "{"
        fprintfn sw "  \"Name\": \"engine\""
        fprintfn sw "}"


        fprintfn sw "]"
        fprintfn sw "}"
        fprintfn sw "}"
        fprintfn sw "}"

        let result : Widget = ofJSON (sw.ToString())
        result.Name |> should equal (Some "Foo") 
        result.Age |> should equal (Some 31L) 
        result.Male |> should equal (Some true) 
        result.Parts |> should equal (Some { BigParts = List.empty; SmallParts = [ { Name = "engine" } ] }) 

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