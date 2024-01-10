// See https://aka.ms/new-console-template for more information
using ConsoleApp1.KomorebiController;
using ConsoleApp1.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using System.Diagnostics;

Console.WriteLine("Hello, World!");


PipeServer server = new PipeServer();
server.Create();

CommandSender.Subscribe(PipeServer.pipeName);

await server.pipeServer.WaitForConnectionAsync();

StreamReader sr = new StreamReader(server.pipeServer);
while (!sr.EndOfStream)
{

    var line = sr.ReadLine();
	try
    {
        JsonDataRoot data = JsonConvert.DeserializeObject<JsonDataRoot>(line);


        Console.WriteLine($"State changed:{data.@event.content}");

        windows_item item;

        if (data.@event.type== "FocusChange")
        {
            JArray ja = JArray.FromObject(data.@event.content);
            var itemObj = ja[1];
            item = JsonConvert.DeserializeObject<windows_item>(itemObj.ToString());
        }

        Console.WriteLine($"Current workspace:{data.state.monitors.elements[0].workspaces.focused}");


    }
    catch (Exception ex)
    {
        //Debugger.Break();
        Console.WriteLine($"Error: {ex.Message}");

    }
}


//var schemaData = File.ReadAllText("C:\\Users\\swetyPC\\Desktop\\schema.txt");
//var schema = await JsonSchema.FromJsonAsync(schemaData);
//var generator = new CSharpGenerator(schema);
//var file = generator.GenerateFile();
//Console.WriteLine(file.ToString());



