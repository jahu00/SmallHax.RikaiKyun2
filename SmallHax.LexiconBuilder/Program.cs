using SmallHax.SimpleLexicon.Parsers;
using SmallHax.SimpleLexicon.Service;
using System.Text;

using (var stream = File.OpenRead("deinflect"))
{
    var deinflectService = new DeinflectSerive(stream);
    var word = "たべられなくて";
    var lookups = deinflectService.Deinflect(word);
    lookups = deinflectService.CleanLookups(lookups);
    return;
}

var service = new EdictStringParser();

using (var stream = File.OpenRead("edict"))
{
    var encoding = CodePagesEncodingProvider.Instance.GetEncoding("euc-jp");
    /*var index = await service.BuildIndex(stream, encoding);
    await service.SaveIndex(index, "index_euc-jp", encoding);
    stream.Seek(0, SeekOrigin.Begin);*/
    var reader = new StreamReader(stream, encoding);
    var text = reader.ReadToEnd();
    File.WriteAllText("edict_utf8", text, Encoding.UTF8);
}
using (var stream = File.OpenRead("edict_utf8"))
{
    var index = await service.BuildIndex(stream, Encoding.UTF8);
    await service.SaveIndex(index, "index_string", Encoding.UTF8);
}
return;