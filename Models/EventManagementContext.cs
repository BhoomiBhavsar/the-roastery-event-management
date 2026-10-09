using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EventMVC.Models;

public partial class EventManagementContext : DbContext
{
    public EventManagementContext()
    {
    }

    public EventManagementContext(DbContextOptions<EventManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AddOn> AddOns { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<BookingAddOn> BookingAddOns { get; set; }

    public virtual DbSet<BookingDecoration> BookingDecorations { get; set; }

    public virtual DbSet<BookingMenuItem> BookingMenuItems { get; set; }

    public virtual DbSet<Decoration> Decorations { get; set; }

    public virtual DbSet<EventBooking> EventBookings { get; set; }

    public virtual DbSet<EventPackage> EventPackages { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }
    public virtual DbSet<Inquiry> Inquiries { get; set; }

    public virtual DbSet<InquiryReply> InquiryReplies { get; set; }
    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<PackageAddOn> PackageAddOns { get; set; }

    public virtual DbSet<PackageDecoration> PackageDecorations { get; set; }

    public virtual DbSet<PackageMenuItem> PackageMenuItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    //public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ASHAVI\\SQLEXPRESS;Initial Catalog=EventManagement;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUser>()
       .ToTable("AspNetUsers");

        modelBuilder.Entity<AddOn>(entity =>
        {
            entity.HasKey(e => e.AddOnId).HasName("PK__AddOns__682701440EAC9CA4");

            entity.Property(e => e.AddOnName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__Areas__70B8204823CC04D6");

            entity.Property(e => e.AreaName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PricePerGuest).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<BookingAddOn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookingA__3214EC07B14BA1BB");

            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.AddOn).WithMany(p => p.BookingAddOns)
                .HasForeignKey(d => d.AddOnId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookingAd__AddOn__6A30C649");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingAddOns)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookingAd__Booki__693CA210");
        });

        modelBuilder.Entity<BookingDecoration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookingD__3214EC078F7DBA50");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingDecorations)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookingDe__Booki__6477ECF3");

            entity.HasOne(d => d.Decoration).WithMany(p => p.BookingDecorations)
                .HasForeignKey(d => d.DecorationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookingDe__Decor__656C112C");
        });

        modelBuilder.Entity<BookingMenuItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookingM__3214EC076BD6189C");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingMenuItems)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookingMe__Booki__60A75C0F");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.BookingMenuItems)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookingMe__MenuI__619B8048");
        });

        modelBuilder.Entity<Decoration>(entity =>
        {
            entity.HasKey(e => e.DecorationId).HasName("PK__Decorati__4EF422BC3CBA19DB");

            entity.Property(e => e.DecorationName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<EventBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__EventBoo__73951AED9B5C809F");

            entity.Property(e => e.BookingMode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.BookingStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SubTotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId)
    .HasMaxLength(450);
            entity.HasOne(d => d.Area).WithMany(p => p.EventBookings)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventBook__AreaI__5DCAEF64");

            entity.HasOne(d => d.EventType).WithMany(p => p.EventBookings)
                .HasForeignKey(d => d.EventTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventBook__Event__5BE2A6F2");

            entity.HasOne(d => d.Package).WithMany(p => p.EventBookings)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK__EventBook__Packa__5CD6CB2B");

            //entity.HasOne(d => d.User).WithMany(p => p.EventBookings)
            //    .HasForeignKey(d => d.UserId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_EventBookings_AspNetUsers_UserId");

            entity.HasOne(d => d.User)
              .WithMany()
              .HasForeignKey(d => d.UserId)
              .HasConstraintName("FK_EventBookings_AspNetUsers_UserId");

        });

        modelBuilder.Entity<EventPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__EventPac__322035CC792CE5B4");

            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PackageName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PricePerGuest).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.EventType).WithMany(p => p.EventPackages)
                .HasForeignKey(d => d.EventTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventPack__Event__4BAC3F29");
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasKey(e => e.EventTypeId).HasName("PK__EventTyp__A9216B3F84FC599A");

            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });


        modelBuilder.Entity<Inquiry>(entity =>
        {
            entity.HasKey(e => e.InquiryId);

            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.Message)
                .HasMaxLength(1000);

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(GETDATE())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.EventType)
                .WithMany()
                .HasForeignKey(d => d.EventTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<InquiryReply>(entity =>
        {
            entity.HasKey(e => e.ReplyId);

            entity.Property(e => e.ReplyMessage)
                .HasMaxLength(2000);

            entity.Property(e => e.RepliedBy)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.Property(e => e.RepliedAt)
                .HasDefaultValueSql("(GETDATE())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Inquiry)
                .WithMany(p => p.InquiryReplies)
                .HasForeignKey(d => d.InquiryId)
                .OnDelete(DeleteBehavior.Cascade);
        });





        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoices__D796AAB5DABC9206");

            entity.Property(e => e.InvoiceDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Booking).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoices__Bookin__71D1E811");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.MenuItemId).HasName("PK__MenuItem__8943F722B50B63DD");

            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ItemName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<PackageAddOn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PackageA__3214EC07479A3098");

            entity.HasOne(d => d.AddOn).WithMany(p => p.PackageAddOns)
                .HasForeignKey(d => d.AddOnId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PackageAd__AddOn__571DF1D5");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageAddOns)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PackageAd__Packa__5629CD9C");
        });

        modelBuilder.Entity<PackageDecoration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PackageD__3214EC07BF3FDBCD");

            entity.HasOne(d => d.Decoration).WithMany(p => p.PackageDecorations)
                .HasForeignKey(d => d.DecorationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PackageDe__Decor__534D60F1");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageDecorations)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PackageDe__Packa__52593CB8");
        });

        modelBuilder.Entity<PackageMenuItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PackageM__3214EC076E5803CC");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.PackageMenuItems)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PackageMe__MenuI__4F7CD00D");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageMenuItems)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PackageMe__Packa__4E88ABD4");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38BC34BDBB");

            entity.Property(e => e.AmountPaid).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaidAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Bookin__6E01572D");
        });

        //modelBuilder.Entity<User>(entity =>
        //{
        //    entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C78BD7F38");

        //    entity.HasIndex(e => e.Email, "UQ__Users__A9D10534FC5D91CB").IsUnique();

        //    entity.Property(e => e.CreatedAt)
        //        .HasDefaultValueSql("(getdate())")
        //        .HasColumnType("datetime");
        //    entity.Property(e => e.Email)
        //        .HasMaxLength(150)
        //        .IsUnicode(false);
        //    entity.Property(e => e.FullName)
        //        .HasMaxLength(150)
        //        .IsUnicode(false);
        //    entity.Property(e => e.IsActive).HasDefaultValue(true);
        //    entity.Property(e => e.PasswordHash)
        //        .HasMaxLength(255)
        //        .IsUnicode(false);
        //    entity.Property(e => e.PhoneNumber)
        //        .HasMaxLength(15)
        //        .IsUnicode(false);
        //    entity.Property(e => e.Role)
        //        .HasMaxLength(20)
        //        .IsUnicode(false);
        //});   

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
