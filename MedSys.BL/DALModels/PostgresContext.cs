using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedSys.BL.DALModels;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Checkup> Checkups { get; set; }

    public virtual DbSet<CheckupType> CheckupTypes { get; set; }

    public virtual DbSet<Disease> Diseases { get; set; }

    public virtual DbSet<Drug> Drugs { get; set; }

    public virtual DbSet<MedicalDocument> MedicalDocuments { get; set; }

    public virtual DbSet<MedicalHistory> MedicalHistories { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
                .UseNpgsql("name=ConnectionStrings:AppConnStr")
                .UseLazyLoadingProxies()
                .EnableSensitiveDataLogging();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_stat_statements");

        modelBuilder.Entity<Checkup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("checkup_pkey");

            entity.ToTable("checkup");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('checkup_checkup_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CheckupTypeId).HasColumnName("checkup_type_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.Time).HasColumnName("time");

            entity.HasOne(d => d.CheckupType).WithMany(p => p.Checkups)
                .HasForeignKey(d => d.CheckupTypeId)
                .HasConstraintName("checkup_checkup_type_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.Checkups)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("checkup_patient_id_fkey");
        });

        modelBuilder.Entity<CheckupType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("checkuptype_pkey");

            entity.ToTable("checkuptype");

            entity.HasIndex(e => e.Code, "checkuptype_code_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('checkuptype_checkup_type_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Disease>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("disease_pkey");

            entity.ToTable("disease");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('disease_disease_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Drug>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("drug_pkey");

            entity.ToTable("drug");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('drug_drug_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MedicalDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("medicaldocument_pkey");

            entity.ToTable("medicaldocument");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('medicaldocument_document_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CheckupId).HasColumnName("checkup_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .HasColumnName("file_path");
            entity.Property(e => e.FileData).HasColumnName("filedata");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Text).HasColumnName("text");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Checkup).WithMany(p => p.MedicalDocuments)
                .HasForeignKey(d => d.CheckupId)
                .HasConstraintName("medicaldocument_checkup_id_fkey");
        });

        modelBuilder.Entity<MedicalHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("medicalhistory_pkey");

            entity.ToTable("medicalhistory");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('medicalhistory_med_history_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.DiseaseId).HasColumnName("disease_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Disease).WithMany(p => p.MedicalHistories)
                .HasForeignKey(d => d.DiseaseId)
                .HasConstraintName("medicalhistory_disease_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicalHistories)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("medicalhistory_patient_id_fkey");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("patient_pkey");

            entity.ToTable("patient");

            entity.HasIndex(e => e.Oib, "patient_oib_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('patient_patient_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Oib)
                .HasMaxLength(20)
                .HasColumnName("oib");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("prescription_pkey");

            entity.ToTable("prescription");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('prescription_prescription_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Dosage)
                .HasMaxLength(50)
                .HasColumnName("dosage");
            entity.Property(e => e.DrugId).HasColumnName("drug_id");
            entity.Property(e => e.Duration)
                .HasMaxLength(50)
                .HasColumnName("duration");
            entity.Property(e => e.Frequency)
                .HasMaxLength(50)
                .HasColumnName("frequency");
            entity.Property(e => e.IssueDate).HasColumnName("issue_date");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");

            entity.HasOne(d => d.Drug).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.DrugId)
                .HasConstraintName("prescription_drug_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("prescription_patient_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
