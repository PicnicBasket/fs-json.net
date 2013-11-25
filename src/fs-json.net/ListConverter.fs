// source : https://gist.github.com/eulerfx/4464543/raw/b1f675f0f72bd12f8a040d065e989f0b745cd0be/ListConverter.fs
namespace Newtonsoft.Json.FSharp
open System
open System.Collections.Generic
open Microsoft.FSharp.Reflection
open Newtonsoft.Json
open Newtonsoft.Json.Converters

type ListConverter() =
    inherit JsonConverter()
    
    override x.CanConvert(t:Type) = 
        t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<list<_>>

    override x.WriteJson(writer, value, serializer) =
        let list = value :?> System.Collections.IEnumerable |> Seq.cast
        serializer.Serialize(writer, list)

    override x.ReadJson(reader, t, _, serializer) = 
        let itemType = t.GetGenericArguments().[0]
        let collectionType = typedefof<IEnumerable<_>>.MakeGenericType(itemType)
        let deserializedList = serializer.Deserialize(reader, collectionType)
        let collection = deserializedList :?> IEnumerable<System.Object>        
        let listType = typedefof<list<_>>.MakeGenericType(itemType)
        let cases = FSharpType.GetUnionCases(listType)
        let rec make = function
            | [] -> FSharpValue.MakeUnion(cases.[0], [||])
            | head::tail -> FSharpValue.MakeUnion(cases.[1], [| head; (make tail); |])                    
        if (collection = null) then
            make(List.empty)
        else
            make (collection |> Seq.toList)