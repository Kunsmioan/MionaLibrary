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

    public virtual DbSet<BookRequest> BookRequests { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<ReturnRequest> ReturnRequests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server =ADMIN\\DUNGTT; database = LibraryManager; uid=sa;pwd=123;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC0719DD2160");

            entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EAC5D28C2C").IsUnique();

            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK_Books_Genres");

            entity.HasOne(d => d.LanguageNavigation).WithMany(p => p.Books)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Books_Languages");
        });

        modelBuilder.Entity<BookRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__BookRequ__33A8519A257BE523");

            entity.ToTable("BookRequest");

            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.Announce).HasMaxLength(255);
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.StatusColor).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany(p => p.BookRequests)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookRequest_Book");

            entity.HasOne(d => d.User).WithMany(p => p.BookRequests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookRequest_User");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genres__3214EC0781FAB212");

            entity.HasIndex(e => e.Name, "UQ__Genres__737584F618FFDBD3").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Language__3214EC078B1452FD");

            entity.HasIndex(e => e.Name, "UQ__Language__737584F63602EDE6").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Loans__3214EC0779744CFE");

            entity.Property(e => e.BorrowDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DueDate)
                .HasDefaultValueSql("(dateadd(day,(7),getdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Borrowing");

            entity.HasOne(d => d.Book).WithMany(p => p.Loans)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Loans__BookId__3C69FB99");

            entity.HasOne(d => d.User).WithMany(p => p.Loans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Loans__UserId__3B75D760");
        });

        modelBuilder.Entity<ReturnRequest>(entity =>
        {
            entity.HasKey(e => e.ReturnRequestId).HasName("PK__ReturnRe__0CCD25992ACCD63D");

            entity.ToTable("ReturnRequest");

            entity.Property(e => e.Announce).HasMaxLength(255);
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Book).WithMany(p => p.ReturnRequests)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReturnRequest_Book");

            entity.HasOne(d => d.Loan).WithMany(p => p.ReturnRequests)
                .HasForeignKey(d => d.LoanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReturnRequest_Loan");

            entity.HasOne(d => d.User).WithMany(p => p.ReturnRequests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReturnRequest_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07584CE635");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E42350B7C5").IsUnique();

            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
