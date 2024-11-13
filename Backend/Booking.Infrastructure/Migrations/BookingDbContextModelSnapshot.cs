﻿// <auto-generated />
using System;
using Booking.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Booking.Infrastructure.Migrations
{
    [DbContext(typeof(BookingDbContext))]
    partial class BookingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Booking.Core.Entities.DynamicFieldType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_dynamic_field_types");

                    b.ToTable("dynamic_field_types", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("ab690aad-5593-48e3-b44e-052cb3d06781"),
                            Title = "Строка"
                        },
                        new
                        {
                            Id = new Guid("5132aff9-3662-42e0-875a-fe4e83ad0948"),
                            Title = "Число"
                        });
                });

            modelBuilder.Entity("Booking.Core.Entities.EntryFieldValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("DynamicFieldId")
                        .HasColumnType("uuid")
                        .HasColumnName("dynamic_field_id");

                    b.Property<Guid>("EventSignupEntryId")
                        .HasColumnType("uuid")
                        .HasColumnName("event_signup_entry_id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_entry_field_values");

                    b.HasIndex("DynamicFieldId")
                        .HasDatabaseName("ix_entry_field_values_dynamic_field_id");

                    b.HasIndex("EventSignupEntryId")
                        .HasDatabaseName("ix_entry_field_values_event_signup_entry_id");

                    b.ToTable("entry_field_values", (string)null);
                });

            modelBuilder.Entity("Booking.Core.Entities.EventSignupEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Fio")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("fio");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone");

                    b.Property<Guid>("SignupWindowId")
                        .HasColumnType("uuid")
                        .HasColumnName("signup_window_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_event_signup_entries");

                    b.HasIndex("SignupWindowId")
                        .HasDatabaseName("ix_event_signup_entries_signup_window_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_event_signup_entries_user_id");

                    b.ToTable("event_signup_entries", (string)null);
                });

            modelBuilder.Entity("Booking.Core.Entities.EventSignupForm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid")
                        .HasColumnName("event_id");

                    b.Property<bool>("IsEmailRequired")
                        .HasColumnType("boolean")
                        .HasColumnName("is_email_required");

                    b.Property<bool>("IsFioRequired")
                        .HasColumnType("boolean")
                        .HasColumnName("is_fio_required");

                    b.Property<bool>("IsPhoneRequired")
                        .HasColumnType("boolean")
                        .HasColumnName("is_phone_required");

                    b.HasKey("Id")
                        .HasName("pk_event_signup_forms");

                    b.HasIndex("EventId")
                        .HasDatabaseName("ix_event_signup_forms_event_id");

                    b.ToTable("event_signup_forms", (string)null);
                });

            modelBuilder.Entity("Booking.Core.Entities.EventSignupWindow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid")
                        .HasColumnName("event_id");

                    b.Property<int>("MaxVisitors")
                        .HasColumnType("integer")
                        .HasColumnName("max_visitors");

                    b.Property<int>("TicketsLeft")
                        .HasColumnType("integer")
                        .HasColumnName("tickets_left");

                    b.Property<TimeOnly>("Time")
                        .HasColumnType("time without time zone")
                        .HasColumnName("time");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_event_signup_windows");

                    b.HasIndex("EventId")
                        .HasDatabaseName("ix_event_signup_windows_event_id");

                    b.ToTable("event_signup_windows", (string)null);
                });

            modelBuilder.Entity("Booking.Core.Entities.FormDynamicField", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("EventFormId")
                        .HasColumnType("uuid")
                        .HasColumnName("event_form_id");

                    b.Property<Guid>("FieldTypeId")
                        .HasColumnType("uuid")
                        .HasColumnName("field_type_id");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("boolean")
                        .HasColumnName("is_required");

                    b.Property<int>("MaxSymbols")
                        .HasColumnType("integer")
                        .HasColumnName("max_symbols");

                    b.Property<string>("MaxValue")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("max_value");

                    b.Property<string>("MinValue")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("min_value");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_form_dynamic_fields");

                    b.HasIndex("EventFormId")
                        .HasDatabaseName("ix_form_dynamic_fields_event_form_id");

                    b.HasIndex("FieldTypeId")
                        .HasDatabaseName("ix_form_dynamic_fields_field_type_id");

                    b.ToTable("form_dynamic_fields", (string)null);
                });

            modelBuilder.Entity("Booking.Core.Entities.OrganizerContacts", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid")
                        .HasColumnName("event_id");

                    b.Property<string>("Fio")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("fio");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone");

                    b.Property<string>("Telegram")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("telegram");

                    b.HasKey("Id")
                        .HasName("pk_organizer_contacts");

                    b.HasIndex("EventId")
                        .HasDatabaseName("ix_organizer_contacts_event_id");

                    b.ToTable("organizer_contacts", (string)null);
                });

            modelBuilder.Entity("Booking.Core.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AvatarImageFilepath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("avatar_image_filepath");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("city");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Fio")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("fio");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.Property<string>("UserStatus")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_status");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_users_role_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Booking.Core.Entities.UserEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("address");

                    b.Property<string>("BannerImageFilepath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("banner_image_filepath");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("city");

                    b.Property<Guid>("CreatorUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("creator_user_id");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_end");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_start");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("boolean")
                        .HasColumnName("is_online");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("boolean")
                        .HasColumnName("is_public");

                    b.Property<bool>("IsSignupOpened")
                        .HasColumnType("boolean")
                        .HasColumnName("is_signup_opened");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_users_events");

                    b.HasIndex("CreatorUserId")
                        .HasDatabaseName("ix_users_events_creator_user_id");

                    b.ToTable("users_events", (string)null);
                });

            modelBuilder.Entity("Booking.Core.Entities.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("CanEditOthersEvents")
                        .HasColumnType("boolean")
                        .HasColumnName("can_edit_others_events");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean")
                        .HasColumnName("is_admin");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_users_roles");

                    b.ToTable("users_roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("2748ce68-6605-4c53-9ee7-9fbb317b2783"),
                            CanEditOthersEvents = false,
                            IsAdmin = false,
                            Title = "User"
                        },
                        new
                        {
                            Id = new Guid("2771367f-d4d8-4cac-a6a6-a107d59a4189"),
                            CanEditOthersEvents = true,
                            IsAdmin = true,
                            Title = "Admin"
                        },
                        new
                        {
                            Id = new Guid("e88670ad-14a9-4e74-aa5d-9dcc9dab3cf8"),
                            CanEditOthersEvents = true,
                            IsAdmin = false,
                            Title = "Moderator"
                        });
                });

            modelBuilder.Entity("Booking.Core.Entities.EntryFieldValue", b =>
                {
                    b.HasOne("Booking.Core.Entities.FormDynamicField", "DynamicField")
                        .WithMany()
                        .HasForeignKey("DynamicFieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_entry_field_values_form_dynamic_fields_dynamic_field_id");

                    b.HasOne("Booking.Core.Entities.EventSignupEntry", "EventSignupEntry")
                        .WithMany()
                        .HasForeignKey("EventSignupEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_entry_field_values_event_signup_entries_event_signup_entry_");

                    b.Navigation("DynamicField");

                    b.Navigation("EventSignupEntry");
                });

            modelBuilder.Entity("Booking.Core.Entities.EventSignupEntry", b =>
                {
                    b.HasOne("Booking.Core.Entities.EventSignupWindow", "SignupWindow")
                        .WithMany()
                        .HasForeignKey("SignupWindowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_event_signup_entries_event_signup_windows_signup_window_id");

                    b.HasOne("Booking.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_event_signup_entries_users_user_id");

                    b.Navigation("SignupWindow");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Booking.Core.Entities.EventSignupForm", b =>
                {
                    b.HasOne("Booking.Core.Entities.UserEvent", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_event_signup_forms_users_events_event_id");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Booking.Core.Entities.EventSignupWindow", b =>
                {
                    b.HasOne("Booking.Core.Entities.UserEvent", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_event_signup_windows_users_events_event_id");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Booking.Core.Entities.FormDynamicField", b =>
                {
                    b.HasOne("Booking.Core.Entities.EventSignupForm", "SignupForm")
                        .WithMany()
                        .HasForeignKey("EventFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_form_dynamic_fields_event_signup_forms_event_form_id");

                    b.HasOne("Booking.Core.Entities.DynamicFieldType", "FieldType")
                        .WithMany()
                        .HasForeignKey("FieldTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_form_dynamic_fields_dynamic_field_types_field_type_id");

                    b.Navigation("FieldType");

                    b.Navigation("SignupForm");
                });

            modelBuilder.Entity("Booking.Core.Entities.OrganizerContacts", b =>
                {
                    b.HasOne("Booking.Core.Entities.UserEvent", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_organizer_contacts_users_events_event_id");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Booking.Core.Entities.User", b =>
                {
                    b.HasOne("Booking.Core.Entities.UserRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_users_roles_role_id");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Booking.Core.Entities.UserEvent", b =>
                {
                    b.HasOne("Booking.Core.Entities.User", "CreatorUser")
                        .WithMany()
                        .HasForeignKey("CreatorUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_events_users_creator_user_id");

                    b.Navigation("CreatorUser");
                });
#pragma warning restore 612, 618
        }
    }
}
