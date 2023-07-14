using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SmallHax.Lexicon.Data;
using SmallHax.Lexicon.Importers;
using System.Text;

var connection = new SqliteConnection("Data Source=dictionary.db");
connection.Open();

var dbContextOptionsBuilder = new DbContextOptionsBuilder().UseSqlite(connection);
var dbContextOptions = dbContextOptionsBuilder.Options;
using var dbContext = new LexiconContext(dbContextOptions);
dbContext.Database.EnsureCreated();

var importer = new EdictImporter(dbContext);
var stream = File.OpenRead("edict");
await importer.Import(stream, "euc-jp");