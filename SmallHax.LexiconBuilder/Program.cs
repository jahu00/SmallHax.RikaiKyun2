using SmallHax.SimpleLexicon.Parsers;
using System.Text;

var service = new EditcParser();
using var stream = File.OpenRead("edict");
var encoding = CodePagesEncodingProvider.Instance.GetEncoding("euc-jp");
var index = await service.BuildIndex(stream, encoding);
await service.SaveIndex(index, "index_euc-jp", encoding);
await service.SaveIndex(index, "index", Encoding.UTF8);
return;