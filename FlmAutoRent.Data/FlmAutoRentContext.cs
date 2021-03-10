using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.SqlServer;
using FlmAutoRent.Data.Entities;

namespace FlmAutoRent.Data
{
    public class FlmAutoRentContext : DbContext
    {
        public FlmAutoRentContext(DbContextOptions<FlmAutoRentContext> o) : base(o) { }
        public virtual DbSet<ProfilingGroup> ProfilingGroups { get; set; }
        public virtual DbSet<ProfilingGroupSystemMenu> ProfilingGroupSystemMenus { get; set; }
        public virtual DbSet<ProfilingOperator> ProfilingOperators { get; set; }
        public virtual DbSet<ProfilingOperatorEmail> ProfilingOperatorEmails { get; set; }
        public virtual DbSet<ProfilingOperatorGroup> ProfilingOperatorGroups { get; set; }
        public virtual DbSet<ProfilingOperatorPasswordHistory> ProfilingOperatorPasswordHistories { get; set; }
        public virtual DbSet<ProfilingSystemMenu> ProfilingSystemMenus { get; set; }
        public virtual DbSet<SystemBlackList> SystemBlackLists { get; set; }
        public virtual DbSet<SystemDefaultEmail> SystemDefaultEmails { get; set; }
        public virtual DbSet<SystemEmail> SystemEmails { get; set; }
        public virtual DbSet<SystemEmailAttachment> SystemEmailAttachments { get; set; }
        public virtual DbSet<SystemEmailMessage> SystemEmailMessages { get; set; }
        public virtual DbSet<SystemLog> SystemLogs { get; set; }
        public virtual DbSet<ContentCategory> ContentCategories { get; set; }
        public virtual DbSet<ContentImage> ContentImages { get; set; }
        public virtual DbSet<ContentCategoryImage> ContentCategoryImages { get; set; }
        public virtual DbSet<ContentNews> ContentNews { get; set; }
        public virtual DbSet<ContentNewsImage> ContentNewsImages { get; set; }
        public virtual DbSet<ContentNewsVideo> ContentNewsVideo { get; set; }
        public virtual DbSet<ContentNewsAttachment> ContentNewsAttachments { get; set; }
        public virtual DbSet<ContentCategoryNews> ContentCategoryNews { get; set; }
        public virtual DbSet<SeoIndex> SeoIndex { get; set; }
        public virtual DbSet<ContentVideo> ContentVideos { get; set; }
        public virtual DbSet<ContentAttachment> ContentAttachments { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<VehiclePowerSupply> VehiclePowerSupplies { get; set; }
        public virtual DbSet<VehiclesBrand> VehiclesBrands { get; set; }
        public virtual DbSet<VehiclesImage> VehiclesImages { get; set; }
        public virtual DbSet<VehiclesImagesMapping> VehiclesImagesMappings { get; set; }
        public virtual DbSet<VehiclesMapping> VehiclesMappings { get; set; }
        public virtual DbSet<VehiclesTransmission> VehiclesTransmissions { get; set; }
        public virtual DbSet<People> Peoples { get; set; }
        public virtual DbSet<PeopleDocument> PeopleDocuments { get; set; }
        public virtual DbSet<PeopleDocumentType> PeopleDocumentTypes { get; set; }
        public virtual DbSet<PeopleMessage> PeopleMessages { get; set; }
        public virtual DbSet<Homepage> Homepage { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            optionsBuilder
                .ConfigureWarnings(warnings => 
                    warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProfilingGroup>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name);
                entity.Property(e => e.Data);
                entity.HasMany(e => e.ProfilingOperatorGroups).WithOne(op => op.Groups );
                entity.HasMany(e => e.ProfilingGroupSystemMenus).WithOne(op => op.Groups );
            });

            modelBuilder.Entity<ProfilingGroupSystemMenu>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Groups).WithMany(op => op.ProfilingGroupSystemMenus);
                entity.HasOne(e => e.SystemMenus).WithMany(op => op.ProfilingGroupSystemMenus);
            });

            modelBuilder.Entity<ProfilingOperator>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Guid);
                entity.Property(e => e.Token);
                entity.Property(e => e.Name);
                entity.Property(e => e.Lastname);
                entity.Property(e => e.UserId);
                entity.Property(e => e.Email);
                entity.Property(e => e.Password);
                entity.Property(e => e.PasswordDeadline);
                entity.Property(e => e.PasswordLastEdit);
                entity.Property(e => e.PhoneNr);
                entity.Property(e => e.Enabled);
                entity.Property(e => e.OperatorData);
                entity.Property(e => e.Avatar);
                entity.HasMany(e => e.SystemsLogs).WithOne(op => op.Operators );
                entity.HasMany(e => e.ProfilingOperatorGroups).WithOne(op => op.Operators );
                entity.HasMany(e => e.ProfilingOperatorPasswordHistories).WithOne(op => op.Operators );
                entity.HasMany(e => e.ProfilingOperatorEmails).WithOne(op => op.Operators );
            });
           
            modelBuilder.Entity<ProfilingOperatorEmail>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Operators).WithMany(g => g.ProfilingOperatorEmails );
                entity.HasOne(e => e.SystemEmails).WithMany(m => m.ProfilingOperatorEmails );
                
            });

            modelBuilder.Entity<ProfilingOperatorGroup>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Operators).WithMany(g => g.ProfilingOperatorGroups );
                entity.HasOne(e => e.Groups).WithMany(m => m.ProfilingOperatorGroups );
                
            });

            modelBuilder.Entity<ProfilingOperatorPasswordHistory>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Password);
                entity.Property(e => e.InsertData);
                entity.HasOne(e => e.Operators).WithMany(o => o.ProfilingOperatorPasswordHistories );
                
            });

            modelBuilder.Entity<ProfilingSystemMenu>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CodMenu);
                entity.Property(e => e.Name);
                entity.Property(e => e.Link);
                entity.Property(e => e.Priority);
                entity.Property(e => e.Visible);
                entity.Property(e => e.MenuFatherId);
                entity.Property(e => e.DisplayHeader);
                entity.HasMany(e => e.ProfilingGroupSystemMenus).WithOne(o => o.SystemMenus );
            });
            
            modelBuilder.Entity<SystemBlackList>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type);
                entity.Property(e => e.IpAddress);
                entity.Property(e => e.ReverseDNS);
                entity.Property(e => e.InsertData);
            });

            modelBuilder.Entity<SystemDefaultEmail>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EmailProvider);
                entity.Property(e => e.EmailSmtp);
                entity.Property(e => e.EmailPortSmtp);
                entity.Property(e => e.EmailPop);
                entity.Property(e => e.EmailPortPop);
                entity.Property(e => e.EmailSendUsing);
                entity.Property(e => e.EmailSSL);
                entity.Property(e => e.EmailAutenticate);
            });

            modelBuilder.Entity<SystemEmail>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name);
                entity.Property(e => e.Email);
                entity.Property(e => e.EmailPop);
                entity.Property(e => e.EmailSmtp);
                entity.Property(e => e.EmailPortSmtp);
                entity.Property(e => e.EmailPortPop);
                entity.Property(e => e.EmailSendUsing);
                entity.Property(e => e.EmailUser);
                entity.Property(e => e.EmailPassword);
                entity.Property(e => e.EmailSSL);
                entity.Property(e => e.EmailAuthenticated);
                entity.Property(e => e.EmailEnable);
                entity.Property(e => e.EmailDelete);
                entity.Property(e => e.EmailManage);
                entity.Property(e => e.EmailGuid);
                entity.Property(e => e.EmailDefault);
                entity.Property(e => e.EmailBccDefault);
                entity.Property(e => e.EmailSignature);
                entity.HasMany(e => e.SystemEmailMessages).WithOne(em => em.SystemEmails);
                entity.HasMany(e => e.SystemEmailAttachments).WithOne(ea => ea.SystemEmails);
                entity.HasMany(e => e.ProfilingOperatorEmails).WithOne(oe => oe.SystemEmails);
            });

            modelBuilder.Entity<SystemEmailAttachment>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AttachmentName);
                entity.Property(e => e.DirectionAttachments);
                entity.HasOne(e => e.SystemEmails).WithMany(e => e.SystemEmailAttachments);
            });

            modelBuilder.Entity<SystemEmailMessage>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EmailGuid);
                entity.Property(e => e.EmailData);
                entity.Property(e => e.EmailFrom);
                entity.Property(e => e.EmailTo);
                entity.Property(e => e.EmailCC);
                entity.Property(e => e.EmailBcc);
                entity.Property(e => e.EmailObject);
                entity.Property(e => e.EmailMessage);
                entity.Property(e => e.EmailRead);
                entity.Property(e => e.InOut);
                entity.Property(e => e.EmailAttachments);
                entity.HasOne(e => e.SystemEmails).WithMany(em => em.SystemEmailMessages);
            });
           
            modelBuilder.Entity<SystemLog>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Data);
                entity.Property(e => e.EventType);
                entity.Property(e => e.Value);
                entity.HasOne(e => e.Operators).WithMany(em => em.SystemsLogs);
            });

            modelBuilder.Entity<ContentCategory>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IdFather);
                entity.Property(e => e.Name);
                entity.Property(e => e.Description);
                entity.Property(e => e.Priority);
                entity.Property(e => e.MetaTitle);
                entity.Property(e => e.MetaDescription);
                entity.Property(e => e.PermaLink);
                entity.Property(e => e.Display);
                entity.Property(e => e.OperatorData);
                entity.Property(e => e.IDOperator);
                entity.HasMany(e => e.ContentCategoryNews).WithOne(x => x.ContentCategories );
                entity.HasMany(e => e.ContentCategoryImages).WithOne(x => x.Categories );
            });

            modelBuilder.Entity<ContentNews>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Guid);
                entity.Property(e => e.Title);
                entity.Property(e => e.SubTitle);
                entity.Property(e => e.Summary);
                entity.Property(e => e.News);
                entity.Property(e => e.PermaLink);
                entity.Property(e => e.MetaDescription);
                entity.Property(e => e.MetaTitle);
                entity.Property(e => e.OperatorData);
                entity.Property(e => e.IDOperator);
                entity.Property(e => e.DisplayOnFooter);
                entity.Property(e => e.Display);
                entity.HasMany(e => e.ContentCategoryNews).WithOne(x => x.News);
                entity.HasMany(e => e.ContentNewsImage).WithOne(x => x.ContentNews);
                entity.HasMany(e => e.ContentNewsVideo).WithOne(x => x.ContentNews);
                entity.HasMany(e => e.ContentNewsAttachment).WithOne(x => x.ContentNews);
                entity.HasOne(e => e.SeoIndex).WithOne(x => x.ContentNews);
            });

            modelBuilder.Entity<ContentNewsImage>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ContentNews).WithMany(x => x.ContentNewsImage);
                entity.HasOne(e => e.ContentImage).WithMany(x => x.ContentNewsImage);
                
            });

            modelBuilder.Entity<ContentNewsVideo>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ContentNews).WithMany(x => x.ContentNewsVideo);
                entity.HasOne(e => e.ContentVideo).WithMany(x => x.ContentNewsVideo);
            });

            modelBuilder.Entity<ContentNewsAttachment>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ContentNews).WithMany(x => x.ContentNewsAttachment);
                entity.HasOne(e => e.ContentAttachment).WithMany(x => x.ContentNewsAttachment);
            });

            modelBuilder.Entity<SeoIndex>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description);
                entity.HasOne(e => e.ContentNews).WithOne(x => x.SeoIndex).HasForeignKey<ContentNews>(b => b.SeoIndexRef);
                entity.HasOne(e => e.Vehicle).WithOne(x => x.SeoIndex).HasForeignKey<Vehicle>(b => b.SeoIndexRef);
                entity.HasOne(e => e.VehiclesBrand).WithOne(x => x.SeoIndex).HasForeignKey<VehiclesBrand>(b => b.SeoIndexRef);
            });
            
            modelBuilder.Entity<ContentImage>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title);
                entity.Property(e => e.Description);
                entity.Property(e => e.FilePath);
                entity.Property(e => e.FileName);
                entity.Property(e => e.FileNameOriginal);
                entity.Property(e => e.FileExtenstion);
                entity.Property(e => e.FileWidth);
                entity.Property(e => e.FileHeight);
                entity.Property(e => e.FileSize);
                entity.Property(e => e.OperatorData);
                entity.Property(e => e.IDOperator);
                entity.HasMany(e => e.ContentCategoryImages).WithOne(x => x.Images );
                entity.HasMany(e => e.ContentNewsImage).WithOne(x => x.ContentImage);
                entity.HasOne(e => e.HomepageHeaderImage).WithOne(x => x.HeaderContentImage).HasForeignKey<Homepage>(b => b.HeaderImageId);
                entity.HasOne(e => e.HomepagePresetationImage).WithOne(x => x.PresentationContentImage).HasForeignKey<Homepage>(b => b.PresentationImageId);
            });

            modelBuilder.Entity<ContentCategoryImage>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Categories).WithMany(g => g.ContentCategoryImages );
                entity.HasOne(e => e.Images).WithMany(m => m.ContentCategoryImages );
                
            });

            modelBuilder.Entity<ContentCategoryNews>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ContentCategories).WithMany(x => x.ContentCategoryNews );
                entity.HasOne(e => e.News).WithMany(x => x.ContentCategoryNews); 
                entity.HasOne(e => e.Vehicle).WithMany(x => x.ContentCategoryNews); 
            });

            modelBuilder.Entity<ContentVideo>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title);
                entity.Property(e => e.Description);
                entity.Property(e => e.FilePath);
                entity.Property(e => e.FileName);
                entity.Property(e => e.FileNameOriginal);
                entity.Property(e => e.FileExtenstion);
                entity.Property(e => e.OperatorData);
                entity.Property(e => e.IDOperator);
                entity.HasMany(e => e.ContentNewsVideo).WithOne(x => x.ContentVideo);
            });

            modelBuilder.Entity<ContentAttachment>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title);
                entity.Property(e => e.Description);
                entity.Property(e => e.FilePath);
                entity.Property(e => e.FileName);
                entity.Property(e => e.FileNameOriginal);
                entity.Property(e => e.FileExtenstion);
                entity.Property(e => e.OperatorData);
                entity.Property(e => e.IDOperator);
                entity.HasMany(e => e.ContentNewsAttachment).WithOne(x => x.ContentAttachment);
            });
     
            modelBuilder.Entity<Vehicle>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Model);
                entity.Property(e => e.Description);
                entity.Property(e => e.Cv);
                entity.Property(e => e.Kw);
                entity.Property(e => e.PermaLink);
                entity.Property(e => e.MetaDescription);
                entity.Property(e => e.MetaTitle);
                entity.Property(e => e.OperatorData);
                entity.Property(e => e.IDOperator);
                entity.Property(e => e.FirstStep);
                entity.Property(e => e.SecondStep);
                entity.Property(e => e.ThirdStep);
                entity.Property(e => e.Bookable);
                entity.Property(e => e.DisplayHp);
                entity.Property(e => e.Priority);
                entity.HasMany(e => e.ContentCategoryNews).WithOne(x => x.Vehicle);
                entity.HasMany(e => e.VehiclesImages).WithOne(e => e.Vehicle);
                entity.HasMany(e => e.VehiclesMappings).WithOne(e => e.Vehicles);
                entity.HasMany(e => e.PeopleMessages).WithOne(x => x.Vehicle);
                entity.HasOne(e => e.SeoIndex).WithOne(x => x.Vehicle);
            });

            modelBuilder.Entity<VehiclesImage>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name);
                entity.Property(e => e.Description);
                entity.Property(e => e.Path);
                entity.Property(e => e.Priority);
                entity.HasMany(e => e.VehiclesImages).WithOne(e => e.Image);
            });

            modelBuilder.Entity<VehiclesImagesMapping>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Image).WithMany(e => e.VehiclesImages);
                entity.HasOne(e => e.Vehicle).WithMany(e => e.VehiclesImages);
            });

            modelBuilder.Entity<VehiclePowerSupply>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PowerSupply);
                entity.HasMany(e => e.VehiclesMappings).WithOne(e => e.Supplies);
            });

            modelBuilder.Entity<VehiclesBrand>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BrandName);
                entity.Property(e => e.Description);
                entity.Property(e => e.BrandImagePath);
                entity.Property(e => e.PermaLink);
                entity.Property(e => e.MetaDescription);
                entity.Property(e => e.MetaTitle);
                entity.Property(e => e.OperatorData);
                entity.Property(e => e.IDOperator);
                entity.HasMany(e => e.VehiclesMappings).WithOne(e => e.Brands);
                entity.HasOne(e => e.SeoIndex).WithOne(x => x.VehiclesBrand);
            });
     
            modelBuilder.Entity<VehiclesTransmission>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Trasmission);
                entity.HasMany(e => e.VehiclesMappings).WithOne(e => e.Transmission);
            });

            modelBuilder.Entity<VehiclesMapping>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Brands).WithMany(e => e.VehiclesMappings);
                entity.HasOne(e => e.Transmission).WithMany(e => e.VehiclesMappings);
                entity.HasOne(e => e.Supplies).WithMany(e => e.VehiclesMappings);
                entity.HasOne(e => e.Vehicles).WithMany(e => e.VehiclesMappings);
            });

            modelBuilder.Entity<People>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name);
                entity.Property(e => e.Lastname);
                entity.Property(e => e.Email);
                entity.Property(e => e.PhoneNr);
                entity.HasMany(e => e.PeopleDocuments).WithOne(x => x.People);
                entity.HasMany(e => e.PeopleMessages).WithOne(x => x.People);
            });
            
            modelBuilder.Entity<PeopleDocumentType>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DocumentType);
                entity.HasMany(e => e.PeopleDocuments).WithOne(x => x.DocumentType);
            });

            modelBuilder.Entity<PeopleDocument>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Path);
                entity.HasOne(e => e.People).WithMany(e => e.PeopleDocuments);
                entity.HasOne(e => e.DocumentType).WithMany(e => e.PeopleDocuments);
            });

            modelBuilder.Entity<PeopleMessage>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Message);
                entity.Property(e => e.DateSend);
                entity.HasOne(e => e.Vehicle).WithMany(e => e.PeopleMessages);
                entity.HasOne(e => e.People).WithMany(e => e.PeopleMessages);
            });

            modelBuilder.Entity<Homepage>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.WelcomeMessage);
                entity.Property(e => e.PresentationMessage);
                entity.HasOne(e => e.HeaderContentImage).WithOne(x => x.HomepageHeaderImage);
                entity.HasOne(e => e.PresentationContentImage).WithOne(x => x.HomepagePresetationImage);
            });
        }
    }
}
