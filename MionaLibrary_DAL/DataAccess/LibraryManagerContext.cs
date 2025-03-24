using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MionaLibrary_DAL.Entity;

namespace MionaLibrary_DAL.DataAccess;

public partial class LibraryManagerContext : DbContext
{
    public LibraryManagerContext()
    {
    }

    public LibraryManagerContext(DbContextOptions<LibraryManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookReservation> BookReservations { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<LoanHistory> LoanHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server =ADMIN\\DUNGTT; database = LibraryManager; uid=sa;pwd=123;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC074D4A55D7");

            entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EA5E3E0268").IsUnique();

            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Genre).HasMaxLength(50);
            entity.Property(e => e.ImagePath).HasColumnType("text");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<BookReservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookRese__3214EC07E3DEC75E");

            entity.Property(e => e.ReserveDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Book).WithMany(p => p.BookReservations)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__BookReser__BookI__3F466844");

            entity.HasOne(d => d.User).WithMany(p => p.BookReservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BookReser__UserI__3E52440B");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Loans__3214EC07E08AA0CD");

            entity.ToTable(tb => tb.HasTrigger("trg_UpdateLoanStatus"));

            entity.Property(e => e.BorrowDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DueDate).HasDefaultValueSql("(dateadd(day,(7),getdate()))");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Borrowing");

            entity.HasOne(d => d.Book).WithMany(p => p.Loans)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__Loans__BookId__32E0915F");

            entity.HasOne(d => d.User).WithMany(p => p.Loans)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Loans__UserId__31EC6D26");
        });

        modelBuilder.Entity<LoanHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__LoanHist__4D7B4ADD6802AE42");

            entity.ToTable("LoanHistory");

            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.LoanDate).HasColumnType("datetime");
            entity.Property(e => e.OverdueDays).HasDefaultValue(0);
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany(p => p.LoanHistories)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LoanHisto__BookI__38996AB5");

            entity.HasOne(d => d.User).WithMany(p => p.LoanHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LoanHisto__UserI__37A5467C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07AEB4BA6F");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E464BF6F9F").IsUnique();

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
