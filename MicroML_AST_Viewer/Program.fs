open Giraffe
open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Http
open Newtonsoft.Json

open Parse
open Fun

type Request = {phrase: string}
type Response = {tree: string}

let parseHandler : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            let! reqBody = ctx.BindJsonAsync<Request>()
            let parsedTree = Parse.fromString reqBody.phrase // Assume `Parse.parse` formats the syntax tree
            let response = { tree = sprintf "%A" parsedTree }
            return! json response next ctx
        }

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    builder.Services.AddGiraffe() |> ignore

    let app = builder.Build()
    app.UseStaticFiles() |> ignore

    let webApp =
        choose [
            route "/" >=> htmlFile "./wwwroot/index.html"
            POST >=> route "/parse" >=> parseHandler
            // your other routes
        ]
    app.UseGiraffe(webApp)

    printfn "%A" Parse.e1

    printfn "%A" Parse.e2

    eval Parse.e1 [] |> printfn "%A"    
    eval Parse.e2 [] |> printfn "%A"

    print Parse.e1 |> printfn "%A"
    print Parse.e2 |> printfn "%A"

    app.Run()

    0

