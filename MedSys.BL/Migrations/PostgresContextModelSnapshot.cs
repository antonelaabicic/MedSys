﻿// <auto-generated />
using System;
using MedSys.BL.DALModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MedSys.BL.Migrations
{
    [DbContext(typeof(PostgresContext))]
    partial class PostgresContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "pg_stat_statements");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MedSys.BL.DALModels.Checkup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasDefaultValueSql("nextval('checkup_checkup_id_seq'::regclass)");

                    b.Property<int>("CheckupTypeId")
                        .HasColumnType("integer")
                        .HasColumnName("checkup_type_id");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<int>("PatientId")
                        .HasColumnType("integer")
                        .HasColumnName("patient_id");

                    b.Property<TimeOnly>("Time")
                        .HasColumnType("time without time zone")
                        .HasColumnName("time");

                    b.HasKey("Id")
                        .HasName("checkup_pkey");

                    b.HasIndex("CheckupTypeId");

                    b.HasIndex("PatientId");

                    b.ToTable("checkup", (string)null);
                });

            modelBuilder.Entity("MedSys.BL.DALModels.CheckupType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasDefaultValueSql("nextval('checkuptype_checkup_type_id_seq'::regclass)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("checkuptype_pkey");

                    b.HasIndex(new[] { "Code" }, "checkuptype_code_key")
                        .IsUnique();

                    b.ToTable("checkuptype", (string)null);
                });

            modelBuilder.Entity("MedSys.BL.DALModels.Disease", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasDefaultValueSql("nextval('disease_disease_id_seq'::regclass)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("disease_pkey");

                    b.ToTable("disease", (string)null);
                });

            modelBuilder.Entity("MedSys.BL.DALModels.Drug", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasDefaultValueSql("nextval('drug_drug_id_seq'::regclass)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("drug_pkey");

                    b.ToTable("drug", (string)null);
                });

            modelBuilder.Entity("MedSys.BL.DALModels.MedicalDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasDefaultValueSql("nextval('medicaldocument_document_id_seq'::regclass)");

                    b.Property<int>("CheckupId")
                        .HasColumnType("integer")
                        .HasColumnName("checkup_id");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<byte[]>("FileData")
                        .HasColumnType("bytea")
                        .HasColumnName("filedata");

                    b.Property<string>("FileKey")
                        .HasColumnType("text");

                    b.Property<string>("FilePath")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("file_path");

                    b.Property<string>("Notes")
                        .HasColumnType("text")
                        .HasColumnName("notes");

                    b.Property<string>("Text")
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("medicaldocument_pkey");

                    b.HasIndex("CheckupId");

                    b.ToTable("medicaldocument", (string)null);
                });

            modelBuilder.Entity("MedSys.BL.DALModels.MedicalHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasDefaultValueSql("nextval('medicalhistory_med_history_id_seq'::regclass)");

                    b.Property<int>("DiseaseId")
                        .HasColumnType("integer")
                        .HasColumnName("disease_id");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date")
                        .HasColumnName("end_date");

                    b.Property<int>("PatientId")
                        .HasColumnType("integer")
                        .HasColumnName("patient_id");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("start_date");

                    b.HasKey("Id")
                        .HasName("medicalhistory_pkey");

                    b.HasIndex("DiseaseId");

                    b.HasIndex("PatientId");

                    b.ToTable("medicalhistory", (string)null);
                });

            modelBuilder.Entity("MedSys.BL.DALModels.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasDefaultValueSql("nextval('patient_patient_id_seq'::regclass)");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("first_name");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("gender");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("last_name");

                    b.Property<string>("Oib")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("oib");

                    b.HasKey("Id")
                        .HasName("patient_pkey");

                    b.HasIndex(new[] { "Oib" }, "patient_oib_key")
                        .IsUnique();

                    b.ToTable("patient", (string)null);
                });

            modelBuilder.Entity("MedSys.BL.DALModels.Prescription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasDefaultValueSql("nextval('prescription_prescription_id_seq'::regclass)");

                    b.Property<string>("Dosage")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("dosage");

                    b.Property<int>("DrugId")
                        .HasColumnType("integer")
                        .HasColumnName("drug_id");

                    b.Property<string>("Duration")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("duration");

                    b.Property<string>("Frequency")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("frequency");

                    b.Property<DateOnly>("IssueDate")
                        .HasColumnType("date")
                        .HasColumnName("issue_date");

                    b.Property<int>("PatientId")
                        .HasColumnType("integer")
                        .HasColumnName("patient_id");

                    b.HasKey("Id")
                        .HasName("prescription_pkey");

                    b.HasIndex("DrugId");

                    b.HasIndex("PatientId");

                    b.ToTable("prescription", (string)null);
                });

            modelBuilder.Entity("MedSys.BL.DALModels.Checkup", b =>
                {
                    b.HasOne("MedSys.BL.DALModels.CheckupType", "CheckupType")
                        .WithMany("Checkups")
                        .HasForeignKey("CheckupTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("checkup_checkup_type_id_fkey");

                    b.HasOne("MedSys.BL.DALModels.Patient", "Patient")
                        .WithMany("Checkups")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("checkup_patient_id_fkey");

                    b.Navigation("CheckupType");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("MedSys.BL.DALModels.MedicalDocument", b =>
                {
                    b.HasOne("MedSys.BL.DALModels.Checkup", "Checkup")
                        .WithMany("MedicalDocuments")
                        .HasForeignKey("CheckupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("medicaldocument_checkup_id_fkey");

                    b.Navigation("Checkup");
                });

            modelBuilder.Entity("MedSys.BL.DALModels.MedicalHistory", b =>
                {
                    b.HasOne("MedSys.BL.DALModels.Disease", "Disease")
                        .WithMany("MedicalHistories")
                        .HasForeignKey("DiseaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("medicalhistory_disease_id_fkey");

                    b.HasOne("MedSys.BL.DALModels.Patient", "Patient")
                        .WithMany("MedicalHistories")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("medicalhistory_patient_id_fkey");

                    b.Navigation("Disease");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("MedSys.BL.DALModels.Prescription", b =>
                {
                    b.HasOne("MedSys.BL.DALModels.Drug", "Drug")
                        .WithMany("Prescriptions")
                        .HasForeignKey("DrugId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("prescription_drug_id_fkey");

                    b.HasOne("MedSys.BL.DALModels.Patient", "Patient")
                        .WithMany("Prescriptions")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("prescription_patient_id_fkey");

                    b.Navigation("Drug");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("MedSys.BL.DALModels.Checkup", b =>
                {
                    b.Navigation("MedicalDocuments");
                });

            modelBuilder.Entity("MedSys.BL.DALModels.CheckupType", b =>
                {
                    b.Navigation("Checkups");
                });

            modelBuilder.Entity("MedSys.BL.DALModels.Disease", b =>
                {
                    b.Navigation("MedicalHistories");
                });

            modelBuilder.Entity("MedSys.BL.DALModels.Drug", b =>
                {
                    b.Navigation("Prescriptions");
                });

            modelBuilder.Entity("MedSys.BL.DALModels.Patient", b =>
                {
                    b.Navigation("Checkups");

                    b.Navigation("MedicalHistories");

                    b.Navigation("Prescriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
