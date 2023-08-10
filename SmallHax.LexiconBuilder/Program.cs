using SmallHax.SimpleLexicon.Service;

var service = new EdictDictionaryService();
var index = await service.BuildIndex("edict", "euc-jp");
await service.SaveIndex(index, "index_eux-jp", "euc-jp");
await service.SaveIndex(index, "index");
return;