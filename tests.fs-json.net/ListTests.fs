namespace Tests.Newtonsoft.Json.FSharp
open Newtonsoft.Json
open NUnit.Framework
open Newtonsoft.Json.FSharp
open FsUnit
open System

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

    type SecurityObjectValue = {
        Id: Guid
        //ObjectType: SecurityObjectType
    }

    type FolderPermissions = {
        Read: list<SecurityObjectValue>
        Write: list<SecurityObjectValue>
    }

    type CreateFolderCommand = { 
        Name: string
        Permissions: option<FolderPermissions>
    }

    [<Test>]
    let ``Regression: Deserializing Permissions`` () : unit =
        let value = "{\"folderId\":\"9b261ffb-8041-4e8e-906c-86d549f6f741\",\"name\":\"ANDREW1c\",\"permissions\":{\"read\":[{\"id\":\"9d12ce52-7a95-4b95-9ee8-7d37734ad56f\",\"displayName\":\"All Users\",\"objectType\":\"Tenancy\"}],\"write\":[{\"id\":\"9d12ce52-7a95-4b95-9ee8-7d37734ad56f\",\"displayName\":\"All Users\",\"objectType\":\"Tenancy\"}]}}"
        let result : CreateFolderCommand = ofJSON value
        result.Permissions.Value.Read.Head.Id |> should equal (Guid.Parse("9d12ce52-7a95-4b95-9ee8-7d37734ad56f"))