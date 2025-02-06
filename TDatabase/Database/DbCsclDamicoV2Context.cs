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

    public virtual DbSet<MeteoCondition> MeteoConditions { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<PatInail> PatInails { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionAnswered> QuestionAnswereds { get; set; }

    public virtual DbSet<QuestionChoice> QuestionChoices { get; set; }

    public virtual DbSet<QuestionChosen> QuestionChosens { get; set; }

    public virtual DbSet<ReportedCompany> ReportedCompanies { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<TemplateDescription> TemplateDescriptions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAttachment> UserAttachments { get; set; }

    public virtual DbSet<UserAttachmentType> UserAttachmentTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC27B85F2862");

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
            entity.Property(e => e.IdOrganization).HasColumnName("ID_ORGANIZATION");
            entity.Property(e => e.IdType).HasColumnName("ID_TYPE");
            entity.Property(e => e.Image)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NAME");

            entity.HasOne(d => d.IdDocumentNavigation).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.IdDocument)
                .HasConstraintName("FK__ATTACHMEN__ID_DO__671F4F74");

            entity.HasOne(d => d.IdOrganizationNavigation).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.IdOrganization)
                .HasConstraintName("FK_ATTACHMENT_ORGANIZATION");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.IdType)
                .HasConstraintName("FK__ATTACHMEN__ID_TY__662B2B3B");
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

            entity.HasOne(d => d.IdAttachmentNavigation).WithMany(p => p.AttachmentNotes)
                .HasForeignKey(d => d.IdAttachment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ATTACHMEN__ID_AT__634EBE90");

            entity.HasOne(d => d.IdNoteNavigation).WithMany(p => p.AttachmentNotes)
                .HasForeignKey(d => d.IdNote)
                .HasConstraintName("FK__ATTACHMEN__ID_NO__40F9A68C");
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

            entity.HasOne(d => d.IdAttachmentNavigation).WithMany(p => p.AttachmentQuestions)
                .HasForeignKey(d => d.IdAttachment)
                .HasConstraintName("FK__ATTACHMEN__ID_AT__6442E2C9");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.AttachmentQuestions)
                .HasForeignKey(d => d.IdQuestion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ATTACHMEN__ID_QU__56B3DD81");
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
            entity.Property(e => e.IdOrganization).HasColumnName("ID_ORGANIZATION");
            entity.Property(e => e.Order).HasColumnName("ORDER");
            entity.Property(e => e.Text)
                .IsUnicode(false)
                .HasColumnName("TEXT");

            entity.HasOne(d => d.IdOrganizationNavigation).WithMany(p => p.Categories)
                .HasForeignKey(d => d.IdOrganization)
                .HasConstraintName("FK_CATEGORY_ORGANIZATION");
        });

        modelBuilder.Entity<Choice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CHOICE__3214EC270C589297");

            entity.ToTable("CHOICE");

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
            entity.Property(e => e.IdOrganization).HasColumnName("ID_ORGANIZATION");
            entity.Property(e => e.Reportable).HasColumnName("REPORTABLE");
            entity.Property(e => e.Tag)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TAG");
            entity.Property(e => e.Value)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VALUE");

            entity.HasOne(d => d.IdOrganizationNavigation).WithMany(p => p.Choices)
                .HasForeignKey(d => d.IdOrganization)
                .HasConstraintName("FK_CHOICE_ORGANIZATION");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CLIENT__3214EC27CBC770B6");

            entity.ToTable("CLIENT");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.IdOrganization).HasColumnName("ID_ORGANIZATION");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NAME");

            entity.HasOne(d => d.IdOrganizationNavigation).WithMany(p => p.Clients)
                .HasForeignKey(d => d.IdOrganization)
                .HasConstraintName("FK_CLIENT_ORGANIZATION");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC27F2F32220");

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
            entity.Property(e => e.IdOrganization).HasColumnName("ID_ORGANIZATION");
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
            entity.Property(e => e.PatInail).HasColumnName("PAT_INAIL");
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

            entity.HasOne(d => d.IdOrganizationNavigation).WithMany(p => p.Companies)
                .HasForeignKey(d => d.IdOrganization)
                .HasConstraintName("FK_COMPANY_ORGANIZATION");

            entity.HasOne(d => d.PatInailNavigation).WithMany(p => p.Companies)
                .HasForeignKey(d => d.PatInail)
                .HasConstraintName("FK_COMPANY_PAT_INAIL");
        });

        modelBuilder.Entity<CompanyConstructorSite>(entity =>
        {
            entity.HasKey(e => new { e.IdCompany, e.IdConstructorSite }).HasName("PK__tmp_ms_x__D64E010511BEACFE");

            entity.ToTable("COMPANY_CONSTRUCTOR_SITE");

            entity.Property(e => e.IdCompany).HasColumnName("ID_COMPANY");
            entity.Property(e => e.IdConstructorSite).HasColumnName("ID_CONSTRUCTOR_SITE");
            entity.Property(e => e.JobsDescription)
                .IsUnicode(false)
                .HasColumnName("JOBS_DESCRIPTION");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("NOTE");
            entity.Property(e => e.SubcontractedBy).HasColumnName("SUBCONTRACTED_BY");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.CompanyConstructorSiteIdCompanyNavigations)
                .HasForeignKey(d => d.IdCompany)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__COMPANY_C__ID_CO__0697FACD");

            entity.HasOne(d => d.IdConstructorSiteNavigation).WithMany(p => p.CompanyConstructorSites)
                .HasForeignKey(d => d.IdConstructorSite)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__COMPANY_C__ID_CO__19AACF41");

            entity.HasOne(d => d.SubcontractedByNavigation).WithMany(p => p.CompanyConstructorSiteSubcontractedByNavigations)
                .HasForeignKey(d => d.SubcontractedBy)
                .HasConstraintName("FK_COMPANY_CONSTRUCTOR_SITE_COMPANY");
        });

        modelBuilder.Entity<CompanyDocument>(entity =>
        {
            entity.HasKey(e => new { e.IdDocument, e.IdCompany }).HasName("PK__COMPANY___59D03CB3AE427FFC");

            entity.ToTable("COMPANY_DOCUMENT");

            entity.Property(e => e.IdDocument).HasColumnName("ID_DOCUMENT");
            entity.Property(e => e.IdCompany).HasColumnName("ID_COMPANY");
            entity.Property(e => e.InChargeWorker)
                .IsUnicode(false)
                .HasColumnName("IN_CHARGE_WORKER");
            entity.Property(e => e.Present).HasColumnName("PRESENT");
            entity.Property(e => e.Workers)
                .IsUnicode(false)
                .HasColumnName("WORKERS");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.CompanyDocuments)
                .HasForeignKey(d => d.IdCompany)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__COMPANY_D__ID_CO__078C1F06");

            entity.HasOne(d => d.IdDocumentNavigation).WithMany(p => p.CompanyDocuments)
                .HasForeignKey(d => d.IdDocument)
                .HasConstraintName("FK__COMPANY_D__ID_DO__44CA3770");
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

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.CompanyNotes)
                .HasForeignKey(d => d.IdCompany)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__COMPANY_N__ID_CO__04AFB25B");

            entity.HasOne(d => d.IdNoteNavigation).WithMany(p => p.CompanyNotes)
                .HasForeignKey(d => d.IdNote)
                .HasConstraintName("FK__COMPANY_N__ID_NO__3E1D39E1");
        });

        modelBuilder.Entity<ConstructorSite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC2748314723");

            entity.ToTable("CONSTRUCTOR_SITE");

            entity.HasIndex(e => e.Name, "UQ__tmp_ms_x__D9C1FA00BD4550CD").IsUnique();

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
            entity.Property(e => e.Cse)
                .IsUnicode(false)
                .HasColumnName("CSE");
            entity.Property(e => e.Dl)
                .IsUnicode(false)
                .HasColumnName("DL");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("END_DATE");
            entity.Property(e => e.IdClient).HasColumnName("ID_CLIENT");
            entity.Property(e => e.IdOrganization).HasColumnName("ID_ORGANIZATION");
            entity.Property(e => e.IdSico)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ID_SICO");
            entity.Property(e => e.IdSicoInProgress)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ID_SICO_IN_PROGRESS");
            entity.Property(e => e.JobDescription)
                .IsUnicode(false)
                .HasColumnName("JOB_DESCRIPTION");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("NOTE");
            entity.Property(e => e.PreliminaryNotificationInProgress)
                .HasColumnType("datetime")
                .HasColumnName("PRELIMINARY_NOTIFICATION_IN_PROGRESS");
            entity.Property(e => e.PreliminaryNotificationStart)
                .HasColumnType("datetime")
                .HasColumnName("PRELIMINARY_NOTIFICATION_START");
            entity.Property(e => e.Rl)
                .IsUnicode(false)
                .HasColumnName("RL");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("START_DATE");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.ConstructorSites)
                .HasForeignKey(d => d.IdClient)
                .HasConstraintName("FK__CONSTRUCT__ID_CL__18B6AB08");

            entity.HasOne(d => d.IdOrganizationNavigation).WithMany(p => p.ConstructorSites)
                .HasForeignKey(d => d.IdOrganization)
                .HasConstraintName("FK_CONSTRUCTOR_SITE_ORGANIZATION");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC273047651A");

            entity.ToTable("DOCUMENT");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CompilationDate)
                .HasColumnType("datetime")
                .HasColumnName("COMPILATION_DATE");
            entity.Property(e => e.CompletedIn)
                .IsUnicode(false)
                .HasColumnName("COMPLETED_IN");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("CREATION_DATE");
            entity.Property(e => e.Cse)
                .IsUnicode(false)
                .HasColumnName("CSE");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.DraftedIn)
                .IsUnicode(false)
                .HasColumnName("DRAFTED_IN");
            entity.Property(e => e.IdClient).HasColumnName("ID_CLIENT");
            entity.Property(e => e.IdConstructorSite).HasColumnName("ID_CONSTRUCTOR_SITE");
            entity.Property(e => e.IdMeteo).HasColumnName("ID_METEO");
            entity.Property(e => e.IdOrganization).HasColumnName("ID_ORGANIZATION");
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
                .HasConstraintName("FK__DOCUMENT__ID_CLI__245D67DE");

            entity.HasOne(d => d.IdConstructorSiteNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.IdConstructorSite)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DOCUMENT__ID_CON__1A9EF37A");

            entity.HasOne(d => d.IdMeteoNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.IdMeteo)
                .HasConstraintName("FK__DOCUMENT__ID_MET__2739D489");

            entity.HasOne(d => d.IdOrganizationNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.IdOrganization)
                .HasConstraintName("FK_DOCUMENT_ORGANIZATION");

            entity.HasOne(d => d.IdTemplateNavigation).WithMany(p => p.Documents)
                .HasForeignKey(d => d.IdTemplate)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DOCUMENT__ID_TEM__4C6B5938");
        });

        modelBuilder.Entity<MeteoCondition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__METEO_CO__3214EC27A0D6F8CC");

            entity.ToTable("METEO_CONDITION");

            entity.HasIndex(e => e.Description, "UQ__METEO_CO__4193D92E6216CE25").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("NOTE");
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
                .HasConstraintName("FK__NOTE__ID_DOCUMEN__3D2915A8");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.ToTable("ORGANIZATION");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Address)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Hidden).HasColumnName("HIDDEN");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Phone)
                .IsUnicode(false)
                .HasColumnName("PHONE");
        });

        modelBuilder.Entity<PatInail>(entity =>
        {
            entity.ToTable("PAT_INAIL");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Hidden).HasColumnName("HIDDEN");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NAME");
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
            entity.Property(e => e.IdOrganization).HasColumnName("ID_ORGANIZATION");
            entity.Property(e => e.Text)
                .IsUnicode(false)
                .HasColumnName("TEXT");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QUESTION__ID_CAT__29572725");

            entity.HasOne(d => d.IdOrganizationNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.IdOrganization)
                .HasConstraintName("FK_QUESTION_ORGANIZATION");
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
                .HasConstraintName("FK__QUESTION___ID_DO__3B40CD36");

            entity.HasOne(d => d.IdQuestionChosenNavigation).WithMany(p => p.QuestionAnswereds)
                .HasForeignKey(d => d.IdQuestionChosen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QUESTION___ID_QU__59904A2C");
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
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC27C8161326");

            entity.ToTable("QUESTION_CHOSEN");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.IdCategory).HasColumnName("ID_CATEGORY");
            entity.Property(e => e.IdQuestion).HasColumnName("ID_QUESTION");
            entity.Property(e => e.IdTemplate).HasColumnName("ID_TEMPLATE");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("NOTE");
            entity.Property(e => e.Order).HasColumnName("ORDER");
            entity.Property(e => e.OrderCategory).HasColumnName("ORDER_CATEGORY");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.QuestionChosens)
                .HasForeignKey(d => d.IdQuestion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QUESTION___ID_QU__589C25F3");

            entity.HasOne(d => d.IdTemplateNavigation).WithMany(p => p.QuestionChosens)
                .HasForeignKey(d => d.IdTemplate)
                .HasConstraintName("FK__QUESTION___ID_TE__57A801BA");
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
                .HasConstraintName("FK__REPORTED___ID_CO__05A3D694");

            entity.HasOne(d => d.QuestionAnswered).WithMany(p => p.ReportedCompanies)
                .HasForeignKey(d => new { d.IdCurrentChoice, d.IdQuestionChosen, d.IdDocument })
                .HasConstraintName("FK__REPORTED_COMPANY__3C34F16F");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("ROLE");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.DocumentsManagement)
                .HasDefaultValue(true)
                .HasColumnName("DOCUMENTS_MANAGEMENT");
            entity.Property(e => e.Hidden).HasColumnName("HIDDEN");
            entity.Property(e => e.IdOrganization).HasColumnName("ID_ORGANIZATION");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.UsersManagement)
                .HasDefaultValue(true)
                .HasColumnName("USERS_MANAGEMENT");

            entity.HasOne(d => d.IdOrganizationNavigation).WithMany(p => p.Roles)
                .HasForeignKey(d => d.IdOrganization)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ID_ORGANIZATION");
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC2743AEB1A2");

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
            entity.Property(e => e.IdDescription).HasColumnName("ID_DESCRIPTION");
            entity.Property(e => e.IdOrganization).HasColumnName("ID_ORGANIZATION");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("NAME");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("NOTE");
            entity.Property(e => e.Title)
                .IsUnicode(false)
                .HasColumnName("TITLE");

            entity.HasOne(d => d.IdDescriptionNavigation).WithMany(p => p.Templates)
                .HasForeignKey(d => d.IdDescription)
                .HasConstraintName("FK__TEMPLATE__ID_DES__4D5F7D71");

            entity.HasOne(d => d.IdOrganizationNavigation).WithMany(p => p.Templates)
                .HasForeignKey(d => d.IdOrganization)
                .HasConstraintName("FK_TEMPLATE_ORGANIZATION");
        });

        modelBuilder.Entity<TemplateDescription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TEMPLATE__3214EC27756F307B");

            entity.ToTable("TEMPLATE_DESCRIPTION");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active).HasColumnName("ACTIVE");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.IdOrganization).HasColumnName("ID_ORGANIZATION");
            entity.Property(e => e.Title)
                .IsUnicode(false)
                .HasColumnName("TITLE");

            entity.HasOne(d => d.IdOrganizationNavigation).WithMany(p => p.TemplateDescriptions)
                .HasForeignKey(d => d.IdOrganization)
                .HasConstraintName("FK_TEMPLATE_DESCRIPTION_ORGANIZATION");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USER");

            entity.HasIndex(e => e.Email, "UQ__tmp_ms_x__161CF724AF36A7CF").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Hidden).HasColumnName("HIDDEN");
            entity.Property(e => e.IdOrganization).HasColumnName("ID_ORGANIZATION");
            entity.Property(e => e.IdRole).HasColumnName("ID_ROLE");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Phone)
                .IsUnicode(false)
                .HasColumnName("PHONE");
            entity.Property(e => e.ResetToken).HasColumnName("RESET_TOKEN");
            entity.Property(e => e.ResetTokenExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("RESET_TOKEN_EXPIRATION_DATE");
            entity.Property(e => e.Salt)
                .IsUnicode(false)
                .HasColumnName("SALT");
            entity.Property(e => e.Surname)
                .IsUnicode(false)
                .HasColumnName("SURNAME");

            entity.HasOne(d => d.IdOrganizationNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdOrganization)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_ORGANIZATION");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_ROLE");
        });

        modelBuilder.Entity<UserAttachment>(entity =>
        {
            entity.ToTable("USER_ATTACHMENT");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Hidden).HasColumnName("HIDDEN");
            entity.Property(e => e.IdUser).HasColumnName("ID_USER");
            entity.Property(e => e.Path)
                .IsUnicode(false)
                .HasColumnName("PATH");
            entity.Property(e => e.Type).HasColumnName("TYPE");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UserAttachments)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_ATTACHMENT_USER");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.UserAttachments)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_ATTACHMENT_USER_ATTACHMENT_TYPE");
        });

        modelBuilder.Entity<UserAttachmentType>(entity =>
        {
            entity.ToTable("USER_ATTACHMENT_TYPE");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("ACTIVE");
            entity.Property(e => e.Hidden).HasColumnName("HIDDEN");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
