using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TDatabase.Database;

public partial class DbCsclDamicoV2Context : DbContext
{
    public DbCsclDamicoV2Context()
    {
    }

    public DbCsclDamicoV2Context(DbContextOptions<DbCsclDamicoV2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<AttachmentNote> AttachmentNotes { get; set; }

    public virtual DbSet<AttachmentQuestion> AttachmentQuestions { get; set; }

    public virtual DbSet<AttachmentType> AttachmentTypes { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Choice> Choices { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyConstructorSite> CompanyConstructorSites { get; set; }

    public virtual DbSet<CompanyDocument> CompanyDocuments { get; set; }

    public virtual DbSet<CompanyNote> CompanyNotes { get; set; }

    public virtual DbSet<ConstructorSite> ConstructorSites { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionAnswered> QuestionAnswereds { get; set; }

    public virtual DbSet<QuestionChoice> QuestionChoices { get; set; }

    public virtual DbSet<QuestionChosen> QuestionChosens { get; set; }

    public virtual DbSet<ReportedCompany> ReportedCompanies { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=v00rca2-vm.sphostserver.com\\axterisco2019;Initial Catalog=DB_CSCL_DAMICO_V2;User ID=sa;password=AdmP@ss2003;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC273090C9D4");

            entity.ToTable("ATTACHMENT");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.DateTime)
                .HasColumnType("datetime")
                .HasColumnName("DATE_TIME");
            entity.Property(e => e.FilePath)
                .IsUnicode(false)
                .HasColumnName("FILE_PATH");
            entity.Property(e => e.IdDocument).HasColumnName("ID_DOCUMENT");
            entity.Property(e => e.IdType).HasColumnName("ID_TYPE");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NAME");

            entity.HasOne(d => d.IdDocumentNavigation).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.IdDocument)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ATTACHMEN__ID_DO__76969D2E");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.IdType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ATTACHMEN__ID_TY__75A278F5");
        });

        modelBuilder.Entity<AttachmentNote>(entity =>
        {
            entity.HasKey(e => new { e.IdAttachment, e.IdNote }).HasName("PK__ATTACHME__A82BD6C052315FD8");

            entity.ToTable("ATTACHMENT_NOTE");

            entity.Property(e => e.IdAttachment).HasColumnName("ID_ATTACHMENT");
            entity.Property(e => e.IdNote).HasColumnName("ID_NOTE");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<AttachmentQuestion>(entity =>
        {
            entity.HasKey(e => new { e.IdAttachment, e.IdQuestion }).HasName("PK__ATTACHME__81AAF6A7A6DB22FE");

            entity.ToTable("ATTACHMENT_QUESTION");

            entity.Property(e => e.IdAttachment).HasColumnName("ID_ATTACHMENT");
            entity.Property(e => e.IdQuestion).HasColumnName("ID_QUESTION");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Location)
                .IsUnicode(false)
                .HasColumnName("LOCATION");
        });

        modelBuilder.Entity<AttachmentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ATTACHME__3214EC2746E7DEB8");

            entity.ToTable("ATTACHMENT_TYPE");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CATEGORY__3214EC2751AC9DF9");

            entity.ToTable("CATEGORY");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Order).HasColumnName("ORDER");
            entity.Property(e => e.Text)
                .IsUnicode(false)
                .HasColumnName("TEXT");
        });

        modelBuilder.Entity<Choice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CHOICE__3214EC270C589297");

            entity.ToTable("CHOICE");

            entity.HasIndex(e => e.Value, "UQ__CHOICE__44966BCAC68CF288").IsUnique();

            entity.HasIndex(e => e.Tag, "UQ__CHOICE__C456903B1564D97D").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Color)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("COLOR");
            entity.Property(e => e.Reportable).HasColumnName("REPORTABLE");
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
            entity.HasKey(e => e.Id).HasName("PK__CLIENT__3214EC27CBC770B6");

            entity.ToTable("CLIENT");

            entity.HasIndex(e => e.Name, "UQ__CLIENT__D9C1FA000CD70BB0").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC27A3DF9A04");

            entity.ToTable("COMPANY");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Ccnl)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("CCNL");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("COMPANY_NAME");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.InailId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("INAIL_ID");
            entity.Property(e => e.InailPat)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("INAIL_PAT");
            entity.Property(e => e.InpsId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("INPS_ID");
            entity.Property(e => e.JobsDescriptions)
                .IsUnicode(false)
                .HasColumnName("JOBS_DESCRIPTIONS");
            entity.Property(e => e.Pec)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PEC");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONE");
            entity.Property(e => e.ReaNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("REA_NUMBER");
            entity.Property(e => e.SelfEmployedName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("SELF_EMPLOYED_NAME");
            entity.Property(e => e.TaxId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TAX_ID");
            entity.Property(e => e.Vatcode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VATCODE");
            entity.Property(e => e.WorkerWelfareFunds)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("WORKER_WELFARE_FUNDS");
        });

        modelBuilder.Entity<CompanyConstructorSite>(entity =>
        {
            entity.HasKey(e => new { e.IdCompany, e.IdConstructorSite }).HasName("PK__COMPANY___D64E0105822D5E16");

            entity.ToTable("COMPANY_CONSTRUCTOR_SITE");

            entity.Property(e => e.IdCompany).HasColumnName("ID_COMPANY");
            entity.Property(e => e.IdConstructorSite).HasColumnName("ID_CONSTRUCTOR_SITE");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("NOTE");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.CompanyConstructorSites)
                .HasForeignKey(d => d.IdCompany)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__COMPANY_C__ID_CO__07C12930");

            entity.HasOne(d => d.IdConstructorSiteNavigation).WithMany(p => p.CompanyConstructorSites)
                .HasForeignKey(d => d.IdConstructorSite)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__COMPANY_C__ID_CO__7C4F7684");
        });

        modelBuilder.Entity<CompanyDocument>(entity =>
        {
            entity.HasKey(e => new { e.IdDocument, e.IdCompany }).HasName("PK__COMPANY___59D03CB3AE427FFC");

            entity.ToTable("COMPANY_DOCUMENT");

            entity.Property(e => e.IdDocument).HasColumnName("ID_DOCUMENT");
            entity.Property(e => e.IdCompany).HasColumnName("ID_COMPANY");
            entity.Property(e => e.Present).HasColumnName("PRESENT");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.CompanyDocuments)
                .HasForeignKey(d => d.IdCompany)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__COMPANY_D__ID_CO__08B54D69");

            entity.HasOne(d => d.IdDocumentNavigation).WithMany(p => p.CompanyDocuments)
                .HasForeignKey(d => d.IdDocument)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__COMPANY_D__ID_DO__5629CD9C");
        });

        modelBuilder.Entity<CompanyNote>(entity =>
        {
            entity.HasKey(e => new { e.IdNote, e.IdCompany }).HasName("PK__COMPANY___64726272262AA39F");

            entity.ToTable("COMPANY_NOTE");

            entity.Property(e => e.IdNote).HasColumnName("ID_NOTE");
            entity.Property(e => e.IdCompany).HasColumnName("ID_COMPANY");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("NOTE");
        });

        modelBuilder.Entity<ConstructorSite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC27A1378A76");

            entity.ToTable("CONSTRUCTOR_SITE");

            entity.HasIndex(e => e.Name, "UQ__tmp_ms_x__D9C1FA00A87E579B").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("END_DATE");
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
                .HasConstraintName("FK__CONSTRUCT__ID_CL__03F0984C");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DOCUMENT__3214EC2755929FAF");

            entity.ToTable("DOCUMENT");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CompilationDate)
                .HasColumnType("datetime")
                .HasColumnName("COMPILATION_DATE");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("CREATION_DATE");
            entity.Property(e => e.IdClient).HasColumnName("ID_CLIENT");
            entity.Property(e => e.IdConstructorSite).HasColumnName("ID_CONSTRUCTOR_SITE");
            entity.Property(e => e.IdTemplate).HasColumnName("ID_TEMPLATE");
            entity.Property(e => e.LastEditDate)
                .HasColumnType("datetime")
                .HasColumnName("LAST_EDIT_DATE");
            entity.Property(e => e.ReadOnly).HasColumnName("READ_ONLY");
            entity.Property(e => e.Title)
                .IsUnicode(false)
                .HasColumnName("TITLE");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.IdClient)
                .HasConstraintName("FK__DOCUMENT__ID_CLI__5165187F");

            entity.HasOne(d => d.IdConstructorSiteNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.IdConstructorSite)
                .HasConstraintName("FK__DOCUMENT__ID_CON__7D439ABD");

            entity.HasOne(d => d.IdTemplateNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.IdTemplate)
                .HasConstraintName("FK__DOCUMENT__ID_TEM__52593CB8");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NOTE__3214EC27C42CB229");

            entity.ToTable("NOTE");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.IdDocument).HasColumnName("ID_DOCUMENT");
            entity.Property(e => e.Text)
                .IsUnicode(false)
                .HasColumnName("TEXT");

            entity.HasOne(d => d.IdDocumentNavigation).WithMany(p => p.Notes)
                .HasForeignKey(d => d.IdDocument)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NOTE__ID_DOCUMEN__71D1E811");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QUESTION__3214EC279BB738DF");

            entity.ToTable("QUESTION");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.IdCategory).HasColumnName("ID_CATEGORY");
            entity.Property(e => e.Text)
                .IsUnicode(false)
                .HasColumnName("TEXT");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QUESTION__ID_CAT__29572725");
        });

        modelBuilder.Entity<QuestionAnswered>(entity =>
        {
            entity.HasKey(e => new { e.IdCurrentChoice, e.IdQuestionChosen, e.IdDocument }).HasName("PK__QUESTION__302B5066ED95C489");

            entity.ToTable("QUESTION_ANSWERED");

            entity.Property(e => e.IdCurrentChoice).HasColumnName("ID_CURRENT_CHOICE");
            entity.Property(e => e.IdQuestionChosen).HasColumnName("ID_QUESTION_CHOSEN");
            entity.Property(e => e.IdDocument).HasColumnName("ID_DOCUMENT");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("NOTE");

            entity.HasOne(d => d.IdCurrentChoiceNavigation).WithMany(p => p.QuestionAnswereds)
                .HasForeignKey(d => d.IdCurrentChoice)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QUESTION___ID_CU__68487DD7");

            entity.HasOne(d => d.IdDocumentNavigation).WithMany(p => p.QuestionAnswereds)
                .HasForeignKey(d => d.IdDocument)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QUESTION___ID_DO__6A30C649");

            entity.HasOne(d => d.IdQuestionChosenNavigation).WithMany(p => p.QuestionAnswereds)
                .HasForeignKey(d => d.IdQuestionChosen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QUESTION___ID_QU__693CA210");
        });

        modelBuilder.Entity<QuestionChoice>(entity =>
        {
            entity.HasKey(e => new { e.IdQuestion, e.IdChoice }).HasName("PK__QUESTION__F037DAD3DDCA2B53");

            entity.ToTable("QUESTION_CHOICE");

            entity.Property(e => e.IdQuestion).HasColumnName("ID_QUESTION");
            entity.Property(e => e.IdChoice).HasColumnName("ID_CHOICE");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("NOTE");

            entity.HasOne(d => d.IdChoiceNavigation).WithMany(p => p.QuestionChoices)
                .HasForeignKey(d => d.IdChoice)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QUESTION___ID_CH__31EC6D26");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.QuestionChoices)
                .HasForeignKey(d => d.IdQuestion)
                .HasConstraintName("FK__QUESTION___ID_QU__32E0915F");
        });

        modelBuilder.Entity<QuestionChosen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QUESTION__3214EC27AD0BD79E");

            entity.ToTable("QUESTION_CHOSEN");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.IdQuestion).HasColumnName("ID_QUESTION");
            entity.Property(e => e.IdTemplate).HasColumnName("ID_TEMPLATE");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("NOTE");
            entity.Property(e => e.Order).HasColumnName("ORDER");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.QuestionChosens)
                .HasForeignKey(d => d.IdQuestion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QUESTION___ID_QU__656C112C");

            entity.HasOne(d => d.IdTemplateNavigation).WithMany(p => p.QuestionChosens)
                .HasForeignKey(d => d.IdTemplate)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QUESTION___ID_TE__6477ECF3");
        });

        modelBuilder.Entity<ReportedCompany>(entity =>
        {
            entity.HasKey(e => new { e.IdCompany, e.IdCurrentChoice, e.IdQuestionChosen, e.IdDocument }).HasName("PK__REPORTED__84A045AB6E482E66");

            entity.ToTable("REPORTED_COMPANY");

            entity.Property(e => e.IdCompany).HasColumnName("ID_COMPANY");
            entity.Property(e => e.IdCurrentChoice).HasColumnName("ID_CURRENT_CHOICE");
            entity.Property(e => e.IdQuestionChosen).HasColumnName("ID_QUESTION_CHOSEN");
            entity.Property(e => e.IdDocument).HasColumnName("ID_DOCUMENT");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("NOTE");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.ReportedCompanies)
                .HasForeignKey(d => d.IdCompany)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__REPORTED___ID_CO__09A971A2");

            entity.HasOne(d => d.QuestionAnswered).WithMany(p => p.ReportedCompanies)
                .HasForeignKey(d => new { d.IdCurrentChoice, d.IdQuestionChosen, d.IdDocument })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__REPORTED_COMPANY__6E01572D");
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TEMPLATE__3214EC27A44F2DEB");

            entity.ToTable("TEMPLATE");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("DATE");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("NOTE");
            entity.Property(e => e.Title)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
