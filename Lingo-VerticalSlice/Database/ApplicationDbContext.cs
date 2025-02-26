using Lingo_VerticalSlice.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lingo_VerticalSlice.Database;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext()
    {
            
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
            
    }
    public DbSet<CardSet> CardSet { get; set; }
    public DbSet<Folder> Folder { get; set; }
    public DbSet<Word> Word { get; set; }
    public DbSet<CardSetWord> CardSetWord { get; set; }
    public DbSet<AnonymousEmail> AnonymousEmail { get; set; }
    public DbSet<WordMeaning> WordMeaning { get; set; }
    public DbSet<WordDefinition> WordDefinition { get; set; }
    public DbSet<WordType> WordType { get; set; }
    public DbSet<WordSynonym> WordSynonym { get; set; }
    public DbSet<WordDefinitionExample> WordDefinitionExample { get; set; }
    public DbSet<SpacedRepetition> SpacedRepetition { get; set; }
    public DbSet<Lesson> Lesson { get; set; }
    public DbSet<Unit> Unit { get; set; }
    public DbSet<UnitWord> UnitWord { get; set; }
    public DbSet<LessonDefinition> LessonDefinition { get; set; }
    public DbSet<VocabularyDetailsMaterializedView> VocabularyDetailsMaterializedView { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
         base.OnModelCreating(modelBuilder);

         modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
         {
             entity.HasKey(x => new { x.LoginProvider, x.ProviderKey });
         });

         modelBuilder.Entity<IdentityUserRole<string>>(entity =>
         {
             entity.HasKey(x => new { x.UserId, x.RoleId });
         });

         modelBuilder.Entity<IdentityUserToken<string>>(entity =>
         {
             entity.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
         });
         modelBuilder.Entity<CardSetWord>()
             .HasOne(csw => csw.Words)
             .WithMany(v => v.CardSetWords)
             .HasForeignKey(csw => csw.VocabularyId)
             .OnDelete(DeleteBehavior.Cascade);
         modelBuilder.Entity<WordDefinition>()
             .HasOne(wd => wd.Word)
             .WithMany(w => w.WordDefinitions)
             .HasForeignKey(wd => wd.VocabularyId)
             .OnDelete(DeleteBehavior.Cascade);
         modelBuilder.Entity<WordDefinition>()
             .HasOne(wd => wd.WordType)
             .WithMany()
             .HasForeignKey(wd => wd.WordTypeId)
             .OnDelete(DeleteBehavior.Cascade);
         modelBuilder.Entity<WordDefinitionExample>()
             .HasOne(wde => wde.WordDefinition)
             .WithMany(wd => wd.WordDefinitionExamples)
             .HasForeignKey(wde => wde.DefinitionId)
             .OnDelete(DeleteBehavior.Cascade);
         modelBuilder.Entity<WordSynonym>()
             .HasOne(wds => wds.WordDefinition)
             .WithMany(wd => wd.Synonyms)
             .HasForeignKey(wds => wds.DefinitionId)
             .OnDelete(DeleteBehavior.Cascade);
         modelBuilder.Entity<WordMeaning>()
             .HasOne(wm => wm.WordDefinition)
             .WithMany(wd => wd.WordMeaning)
             .HasForeignKey(wm => wm.DefinitionId)
             .OnDelete(DeleteBehavior.Cascade);
          modelBuilder.Entity<SpacedRepetition>()
              .HasOne(sp=>sp.Words)
              .WithMany(w=>w.SpacedRepetitions)
              .HasForeignKey(sp=>sp.VocabularyId)
              .OnDelete(DeleteBehavior.Cascade);
          
          modelBuilder.Entity<VocabularyDetailsMaterializedView>()
              .HasKey(v => v.UniqueId);
          modelBuilder.Entity<VocabularyDetailsMaterializedView>()
              .Property(v => v.Example) // Ensure this matches the correct column name in the materialized view
              .HasColumnName("examples");
          modelBuilder.Entity<VocabularyDetailsMaterializedView>()
              .Property(v => v.Synonym) // Ensure this matches the correct column name in the materialized view
              .HasColumnName("synonyms");
          modelBuilder.Entity<VocabularyDetailsMaterializedView>()
              .Property(v => v.VocabularyId) 
              .HasColumnName("vocabularyid");
          modelBuilder.Entity<VocabularyDetailsMaterializedView>()
              .Property(v => v.Definition) 
              .HasColumnName("definition");
          modelBuilder.Entity<VocabularyDetailsMaterializedView>()
              .Property(v => v.PartOfSpeech) 
              .HasColumnName("partofspeech");
          modelBuilder.Entity<VocabularyDetailsMaterializedView>()
              .Property(v => v.UniqueId) 
              .HasColumnName("uniqueid");

     }
}