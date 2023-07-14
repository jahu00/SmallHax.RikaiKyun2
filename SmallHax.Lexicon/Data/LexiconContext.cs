using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHax.Lexicon.Data
{
    public class LexiconContext: DbContext
    {
        public DbSet<Models.LexiconType> LexiconTypes { get; set; }
        public DbSet<Models.Lexicon> Lexicons { get; set; }
        public DbSet<Models.Deinflect> Deinflects { get; set; }
        public DbSet<Models.DeinflectTag> DeinflectTags{ get; set; }
        public DbSet<Models.Definition> Definitions { get; set; }
        public DbSet<Models.Tag> Tags { get; set; }
        public DbSet<Models.DefinitionTag> DefinitionTags { get; set; }
        public DbSet<Models.Reading> Readings { get; set; }
        public DbSet<Models.Entry> Entries { get; set; }
        public DbSet<Models.EntryTag> EntrieTags { get; set; }
        public DbSet<Models.ReadingTag> ReadingTags { get; set; }


        public LexiconContext(DbContextOptions options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Lexicon>(lexicon =>
            {
                lexicon.HasOne(x => x.LexiconType)
                    .WithMany(x => x.Lexicons)
                    .HasForeignKey(x => x.LexiconTypeId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<Models.DeinflectTag>(deinflectTag =>
            {
                deinflectTag.HasOne(x => x.Deinflect)
                    .WithMany(x => x.DeinflectTags)
                    .HasForeignKey(x => x.DeinflectId)
                    .HasPrincipalKey(x => x.Id);

                deinflectTag.HasOne(x => x.Tag)
                    .WithMany(x => x.DeinflectTags)
                    .HasForeignKey(x => x.TagId)
                    .HasPrincipalKey(x => x.Id);

                deinflectTag.HasKey(x => new { x.DeinflectId, x.TagId });
            });

            modelBuilder.Entity<Models.Definition>(definition =>
            {
                definition.HasOne(x => x.Lexicon)
                    .WithMany(x => x.Definitions)
                    .HasForeignKey(x => x.LexiconId)
                    .HasPrincipalKey(x => x.Id);

            });

            modelBuilder.Entity<Models.Tag>(tag =>
            {
                tag.HasOne(x => x.LexiconType)
                    .WithMany(x => x.Tags)
                    .HasForeignKey(x => x.LexiconTypeId)
                    .HasPrincipalKey(x => x.Id);
            });

            modelBuilder.Entity<Models.DefinitionTag>(definitionTag =>
            {
                definitionTag.HasOne(x => x.Definition)
                    .WithMany(x => x.DefinitionTags)
                    .HasForeignKey(x => x.DefinitionId)
                    .HasPrincipalKey(x => x.Id);

                definitionTag.HasOne(x => x.Tag)
                    .WithMany(x => x.DefinitionTags)
                    .HasForeignKey(x => x.TagId)
                    .HasPrincipalKey(x => x.Id);

                definitionTag.HasKey(x => new { x.DefinitionId, x.TagId });
            });

            modelBuilder.Entity<Models.Reading>(reading =>
            {
                reading.HasOne(x => x.Entry)
                    .WithMany(x => x.Readings)
                    .HasForeignKey(x => x.EntryId)
                    .HasPrincipalKey(x => x.Id);

            });

            modelBuilder.Entity<Models.Entry>(entry =>
            {
                entry.HasOne(x => x.Lexicon)
                    .WithMany(x => x.Entries)
                    .HasForeignKey(x => x.LexiconId)
                    .HasPrincipalKey(x => x.Id);

                entry.HasOne(x => x.Definition)
                    .WithMany(x => x.Entries)
                    .HasForeignKey(x => x.DefinitionId)
                    .HasPrincipalKey(x => x.Id);

            });

            modelBuilder.Entity<Models.EntryTag>(entryTag =>
            {
                entryTag.HasOne(x => x.Entry)
                    .WithMany(x => x.EntryTags)
                    .HasForeignKey(x => x.EntryId)
                    .HasPrincipalKey(x => x.Id);

                entryTag.HasOne(x => x.Tag)
                    .WithMany(x => x.EntryTags)
                    .HasForeignKey(x => x.TagId)
                    .HasPrincipalKey(x => x.Id);

                entryTag.HasKey(x => new { x.EntryId, x.TagId });
            });

            modelBuilder.Entity<Models.ReadingTag>(readingTag =>
            {
                readingTag.HasOne(x => x.Reading)
                    .WithMany(x => x.ReadingTags)
                    .HasForeignKey(x => x.ReadingId)
                    .HasPrincipalKey(x => x.Id);

                readingTag.HasOne(x => x.Tag)
                    .WithMany(x => x.ReadingTags)
                    .HasForeignKey(x => x.TagId)
                    .HasPrincipalKey(x => x.Id);

                readingTag.HasKey(x => new { x.ReadingId, x.TagId });
            });

        }
    }
}
