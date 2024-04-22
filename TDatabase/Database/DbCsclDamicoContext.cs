using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TDatabase.Database;

public partial class DbCsclDamicoContext : DbContext
{
    public DbCsclDamicoContext()
    {
    }

    public DbCsclDamicoContext(DbContextOptions<DbCsclDamicoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<AttachmentQuestion> AttachmentQuestions { get; set; }

    public virtual DbSet<Choice> Choices { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<ConstructorSite> ConstructorSites { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<MacroCategory> MacroCategories { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionChosen> QuestionChosens { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ATTACHME__3214EC27EA5659F4");

            entity.ToTable("ATTACHMENT");

            entity.HasIndex(e => e.Name, "UQ__ATTACHME__D9C1FA0075123105").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<AttachmentQuestion>(entity =>
        {
            entity.HasKey(e => new { e.IdAttachment, e.IdQuestion }).HasName("PK__ATTACHME__81AAF6A71DB368B3");

            entity.ToTable("ATTACHMENT_QUESTION");

            entity.Property(e => e.IdAttachment).HasColumnName("ID_ATTACHMENT");
            entity.Property(e => e.IdQuestion).HasColumnName("ID_QUESTION");
            entity.Property(e => e.DateTime)
                .HasColumnType("datetime")
                .HasColumnName("DATE_TIME");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Location)
                .IsUnicode(false)
                .HasColumnName("LOCATION");

            entity.HasOne(d => d.IdAttachmentNavigation).WithMany(p => p.AttachmentQuestions)
                .HasForeignKey(d => d.IdAttachment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ATTACHMEN__ID_AT__4CA06362");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.AttachmentQuestions)
                .HasForeignKey(d => d.IdQuestion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ATTACHMEN__ID_QU__4D94879B");
        });

        modelBuilder.Entity<Choice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CHOICE__3214EC278E51BC42");

            entity.ToTable("CHOICE");

            entity.HasIndex(e => e.Value, "UQ__CHOICE__44966BCAE48597E7").IsUnique();

            entity.HasIndex(e => e.Tag, "UQ__CHOICE__C456903BE7CA446C").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Tag)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TAG");
            entity.Property(e => e.Value)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VALUE");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CLIENT__3214EC27A9A3E2E8");

            entity.ToTable("CLIENT");

            entity.HasIndex(e => e.Name, "UQ__CLIENT__D9C1FA0023B2BC29").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__COMPANY__3214EC2784B69433");

            entity.ToTable("COMPANY");

            entity.HasIndex(e => e.Vatcode, "UQ__COMPANY__675166D95827C13A").IsUnique();

            entity.HasIndex(e => e.Name, "UQ__COMPANY__D9C1FA00A57126D0").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Vatcode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VATCODE");
        });

        modelBuilder.Entity<ConstructorSite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CONSTRUC__3214EC279A0BB333");

            entity.ToTable("CONSTRUCTOR_SITE");

            entity.HasIndex(e => e.Name, "UQ__CONSTRUC__D9C1FA007E37508D").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.IdClient).HasColumnName("ID_CLIENT");
            entity.Property(e => e.JobDescription)
                .IsUnicode(false)
                .HasColumnName("JOB_DESCRIPTION");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("START_DATE");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.ConstructorSites)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CONSTRUCT__ID_CL__4222D4EF");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DOCUMENT__3214EC27F895EDC3");

            entity.ToTable("DOCUMENT");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("DATE");
            entity.Property(e => e.IdClient).HasColumnName("ID_CLIENT");
            entity.Property(e => e.IdConstructorSite).HasColumnName("ID_CONSTRUCTOR_SITE");
            entity.Property(e => e.Title)
                .IsUnicode(false)
                .HasColumnName("TITLE");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DOCUMENT__ID_CLI__45F365D3");

            entity.HasOne(d => d.IdConstructorSiteNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.IdConstructorSite)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DOCUMENT__ID_CON__44FF419A");

            entity.HasMany(d => d.IdCompanies).WithMany(p => p.IdDocuments)
                .UsingEntity<Dictionary<string, object>>(
                    "CompanyDocument",
                    r => r.HasOne<Company>().WithMany()
                        .HasForeignKey("IdCompany")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__COMPANY_D__ID_CO__49C3F6B7"),
                    l => l.HasOne<Document>().WithMany()
                        .HasForeignKey("IdDocument")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__COMPANY_D__ID_DO__48CFD27E"),
                    j =>
                    {
                        j.HasKey("IdDocument", "IdCompany").HasName("PK__COMPANY___59D03CB3B3C3E5F7");
                        j.ToTable("COMPANY_DOCUMENT");
                        j.IndexerProperty<int>("IdDocument").HasColumnName("ID_DOCUMENT");
                        j.IndexerProperty<int>("IdCompany").HasColumnName("ID_COMPANY");
                    });
        });

        modelBuilder.Entity<MacroCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MACRO_CA__3214EC272B89DED4");

            entity.ToTable("MACRO_CATEGORY");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Text)
                .IsUnicode(false)
                .HasColumnName("TEXT");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QUESTION__3214EC2752B2003E");

            entity.ToTable("QUESTION");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.IdSubCategory).HasColumnName("ID_SUB_CATEGORY");
            entity.Property(e => e.Text)
                .IsUnicode(false)
                .HasColumnName("TEXT");

            entity.HasOne(d => d.IdSubCategoryNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.IdSubCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QUESTION__ID_SUB__2C3393D0");

            entity.HasMany(d => d.IdChoices).WithMany(p => p.IdQuestions)
                .UsingEntity<Dictionary<string, object>>(
                    "QuestionChoice",
                    r => r.HasOne<Choice>().WithMany()
                        .HasForeignKey("IdChoice")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__QUESTION___ID_CH__34C8D9D1"),
                    l => l.HasOne<Question>().WithMany()
                        .HasForeignKey("IdQuestion")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__QUESTION___ID_QU__33D4B598"),
                    j =>
                    {
                        j.HasKey("IdQuestion", "IdChoice").HasName("PK__QUESTION__F037DAD3175C9C7F");
                        j.ToTable("QUESTION_CHOICE");
                        j.IndexerProperty<int>("IdQuestion").HasColumnName("ID_QUESTION");
                        j.IndexerProperty<int>("IdChoice").HasColumnName("ID_CHOICE");
                    });
        });

        modelBuilder.Entity<QuestionChosen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QUESTION__3214EC271E06FC8E");

            entity.ToTable("QUESTION_CHOSEN");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Hidden).HasColumnName("HIDDEN");
            entity.Property(e => e.IdCurrentChoice).HasColumnName("ID_CURRENT_CHOICE");
            entity.Property(e => e.IdDocument).HasColumnName("ID_DOCUMENT");
            entity.Property(e => e.IdQuestion).HasColumnName("ID_QUESTION");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("NOTE");
            entity.Property(e => e.Printable)
                .HasDefaultValue(true)
                .HasColumnName("PRINTABLE");

            entity.HasOne(d => d.IdCurrentChoiceNavigation).WithMany(p => p.QuestionChosens)
                .HasForeignKey(d => d.IdCurrentChoice)
                .HasConstraintName("FK__QUESTION___ID_CU__534D60F1");

            entity.HasOne(d => d.IdDocumentNavigation).WithMany(p => p.QuestionChosens)
                .HasForeignKey(d => d.IdDocument)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QUESTION___ID_DO__52593CB8");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.QuestionChosens)
                .HasForeignKey(d => d.IdQuestion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QUESTION___ID_QU__5441852A");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SUB_CATE__3214EC27EB35D049");

            entity.ToTable("SUB_CATEGORY");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.IdMacroCategory).HasColumnName("ID_MACRO_CATEGORY");
            entity.Property(e => e.Text)
                .IsUnicode(false)
                .HasColumnName("TEXT");

            entity.HasOne(d => d.IdMacroCategoryNavigation).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.IdMacroCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SUB_CATEG__ID_MA__286302EC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
