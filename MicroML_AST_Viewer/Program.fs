open Giraffe
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting

let webApp =
    choose [
        route "/" >=> text "Hello from Giraffe!"
    ]

let configureApp (app: IApplicationBuilder) =
    app.UseGiraffe webApp

[<EntryPoint>]
let main args =
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(fun webBuilder ->
            webBuilder.Configure(configureApp))
        .Build()
        .Run()
    0

