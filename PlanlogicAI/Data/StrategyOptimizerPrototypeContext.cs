using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PlanlogicAI.Data
{
    public partial class StrategyOptimizerPrototypeContext : DbContext
    {
        public StrategyOptimizerPrototypeContext()
        {
        }


        public virtual DbSet<Advisor> Advisor { get; set; }
        public virtual DbSet<AlternativeClientFunds> AlternativeClientFunds { get; set; }
        public virtual DbSet<AlternativeClientProducts> AlternativeClientProducts { get; set; }
        public virtual DbSet<AssetTypes> AssetTypes { get; set; }
        public virtual DbSet<BasicDetails> BasicDetails { get; set; }
        public virtual DbSet<CashFlow> CashFlow { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<CostOfAdvice> CostOfAdvice { get; set; }
        public virtual DbSet<CurrentClientFunds> CurrentClientFunds { get; set; }
        public virtual DbSet<CurrentClientProducts> CurrentClientProducts { get; set; }
        public virtual DbSet<CurrentFeeDetails> CurrentFeeDetails { get; set; }
        public virtual DbSet<CurrentIncomeCover> CurrentIncomeCover { get; set; }
        public virtual DbSet<CurrentInsuranceProducts> CurrentInsuranceProducts { get; set; }
        public virtual DbSet<CurrentLifeCover> CurrentLifeCover { get; set; }
        public virtual DbSet<CurrentTpdCover> CurrentTpdCover { get; set; }
        public virtual DbSet<CurrentTraumaCover> CurrentTraumaCover { get; set; }
        public virtual DbSet<General> General { get; set; }
        public virtual DbSet<InsuranceReplacement> InsuranceReplacement { get; set; }
        public virtual DbSet<Investment> Investment { get; set; }
        public virtual DbSet<InvestmentDetails> InvestmentDetails { get; set; }
        public virtual DbSet<InvestmentFund> InvestmentFund { get; set; }
        public virtual DbSet<Liability> Liability { get; set; }
        public virtual DbSet<LiabilityDetails> LiabilityDetails { get; set; }
        public virtual DbSet<LifestyleAsset> LifestyleAsset { get; set; }
        public virtual DbSet<MarginalTaxRates> MarginalTaxRates { get; set; }
        public virtual DbSet<MinimumPensionDrawdown> MinimumPensionDrawdown { get; set; }
        public virtual DbSet<NeedsAnalysis> NeedsAnalysis { get; set; }
        public virtual DbSet<Pension> Pension { get; set; }
        public virtual DbSet<PensionDetails> PensionDetails { get; set; }
        public virtual DbSet<PensionDrawdown> PensionDrawdown { get; set; }
        public virtual DbSet<Platform> Platform { get; set; }
        public virtual DbSet<PreservationAge> PreservationAge { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductFees> ProductFees { get; set; }
        public virtual DbSet<ProductFund> ProductFund { get; set; }
        public virtual DbSet<ProductReplacement> ProductReplacement { get; set; }
        public virtual DbSet<Property> Property { get; set; }
        public virtual DbSet<ProposedClientFunds> ProposedClientFunds { get; set; }
        public virtual DbSet<ProposedClientProducts> ProposedClientProducts { get; set; }
        public virtual DbSet<ProposedFeeDetails> ProposedFeeDetails { get; set; }
        public virtual DbSet<ProposedIncomeCover> ProposedIncomeCover { get; set; }
        public virtual DbSet<ProposedInsuranceProducts> ProposedInsuranceProducts { get; set; }
        public virtual DbSet<ProposedLifeCover> ProposedLifeCover { get; set; }
        public virtual DbSet<ProposedTpdCover> ProposedTpdCover { get; set; }
        public virtual DbSet<ProposedTraumaCover> ProposedTraumaCover { get; set; }
        public virtual DbSet<QualifyingAge> QualifyingAge { get; set; }
        public virtual DbSet<RiskProfile> RiskProfile { get; set; }
        public virtual DbSet<RopcurrentFunds> RopcurrentFunds { get; set; }
        public virtual DbSet<RopcurrentProducts> RopcurrentProducts { get; set; }
        public virtual DbSet<Sgcrate> Sgcrate { get; set; }
        public virtual DbSet<Super> Super { get; set; }
        public virtual DbSet<SuperAssumptions> SuperAssumptions { get; set; }
        public virtual DbSet<SuperDetails> SuperDetails { get; set; }

        public StrategyOptimizerPrototypeContext(DbContextOptions<StrategyOptimizerPrototypeContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advisor>(entity =>
            {
                entity.ToTable("Advisor", "SO");

                entity.Property(e => e.AdvisorName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AlternativeClientFunds>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("AlternativeClientFunds", "PC");

                entity.Property(e => e.Apircode)
                    .IsRequired()
                    .HasColumnName("APIRCode")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Value).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<AlternativeClientProducts>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("AlternativeClientProducts", "PC");

                entity.Property(e => e.RecId).ValueGeneratedNever();

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Value).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<AssetTypes>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("AssetTypes", "AS");

                entity.Property(e => e.Franking).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Growth).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Income).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ProductFees).HasColumnType("decimal(5, 4)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BasicDetails>(entity =>
            {
                entity.HasKey(e => e.ClientId);

                entity.ToTable("BasicDetails", "SO");

                entity.Property(e => e.ClientId).ValueGeneratedNever();

                entity.Property(e => e.ClientDob)
                    .HasColumnName("ClientDOB")
                    .HasColumnType("date");

                entity.Property(e => e.ClientEmpStatus)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ClientPrivateHealthInsurance)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ClientRiskProfile)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.FamilyName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.JointRiskProfile)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.MaritalStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PartnerDob)
                    .HasColumnName("PartnerDOB")
                    .HasColumnType("date");

                entity.Property(e => e.PartnerEmpStatus)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.PartnerName)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.PartnerPrivateHealthInsurance)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PartnerRiskProfile)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CashFlow>(entity =>
            {
                entity.HasKey(e => e.CflowId);

                entity.ToTable("CashFlow", "SO");

                entity.Property(e => e.CflowId).HasColumnName("CFlowId");

                entity.Property(e => e.Cfname)
                    .IsRequired()
                    .HasColumnName("CFName")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Cftype)
                    .IsRequired()
                    .HasColumnName("CFType")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.EndDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Indexation).HasColumnType("decimal(5, 1)");

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.StartDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client", "SO");

                entity.Property(e => e.FamilyName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CostOfAdvice>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("CostOfAdvice", "IN");

                entity.Property(e => e.Adviser).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CoaType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Commission).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Practice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Riadvice)
                    .HasColumnName("RIAdvice")
                    .HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<CurrentClientFunds>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("CurrentClientFunds", "PC");

                entity.Property(e => e.Apircode)
                    .IsRequired()
                    .HasColumnName("APIRCode")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Value).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<CurrentClientProducts>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("CurrentClientProducts", "PC");

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.UnutilizedValue).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Value).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<CurrentFeeDetails>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("CurrentFeeDetails", "IN");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.FeeType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Frequency)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SpecialNotes).IsUnicode(false);
            });

            modelBuilder.Entity<CurrentIncomeCover>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("CurrentIncomeCover", "IN");

                entity.Property(e => e.BenefitPeriod)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Definition)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MonthlyBenefitAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PolicyOwner)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PremiumType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WaitingPeriod)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CurrentInsuranceProducts>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("CurrentInsuranceProducts", "IN");

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Provider)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CurrentLifeCover>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("CurrentLifeCover", "IN");

                entity.Property(e => e.BenefitAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PolicyOwner)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PremiumType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CurrentTpdCover>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("CurrentTpdCover", "IN");

                entity.Property(e => e.BenefitAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Definition)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DisabilityTerm)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PolicyOwner)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PremiumType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StandaloneOrLinked)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WithinSuper)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CurrentTraumaCover>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("CurrentTraumaCover", "IN");

                entity.Property(e => e.BenefitAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PolicyOwner)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PremiumType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StandaloneOrLinked)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WithinSuper)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<General>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("General", "AS");

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 1)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InsuranceReplacement>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("InsuranceReplacement", "IN");
            });

            modelBuilder.Entity<Investment>(entity =>
            {
                entity.ToTable("Investment", "SO");

                entity.Property(e => e.Centrelink)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.EndDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Franked).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Growth).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Income).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.ProductFees).HasColumnType("decimal(5, 4)");

                entity.Property(e => e.Reinvest)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.StartDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvestmentDetails>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("InvestmentDetails", "SO");

                entity.Property(e => e.FromDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ToDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InvestmentFund>(entity =>
            {
                entity.HasKey(e => e.Apircode);

                entity.ToTable("InvestmentFund", "PC");

                entity.Property(e => e.Apircode)
                    .HasColumnName("APIRCode")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.BuySpread).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DefensiveAlternatives).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticCash).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticEquity).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticFixedInterest).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticProperty).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.FundName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.GrowthAlternatives).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Icr)
                    .HasColumnName("ICR")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalCash).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalEquity).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalFixedInterest).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalProperty).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InvestorProfile)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.IsSingle)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Mid)
                    .IsRequired()
                    .HasColumnName("MId")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.OtherGrowth).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.SubType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Liability>(entity =>
            {
                entity.ToTable("Liability", "SO");

                entity.Property(e => e.AssociatedAsset)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CommenceOnDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.RepaymentDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RepaymentType)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LiabilityDetails>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("LiabilityDetails", "SO");

                entity.Property(e => e.FromDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ToDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LifestyleAsset>(entity =>
            {
                entity.HasKey(e => e.LassetId);

                entity.ToTable("LifestyleAsset", "SO");

                entity.Property(e => e.LassetId).HasColumnName("LAssetId");

                entity.Property(e => e.EndDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Growth).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.LassetType)
                    .IsRequired()
                    .HasColumnName("LAssetType")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.StartDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MarginalTaxRates>(entity =>
            {
                entity.HasKey(e => e.Index);

                entity.ToTable("MarginalTaxRates", "AS");

                entity.Property(e => e.Index).ValueGeneratedNever();

                entity.Property(e => e.Rate).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<MinimumPensionDrawdown>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("MinimumPensionDrawdown", "AS");
            });

            modelBuilder.Entity<NeedsAnalysis>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("NeedsAnalysis", "IN");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.IncomeProtection)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Life)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Owner)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Tpd)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Trauma)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pension>(entity =>
            {
                entity.ToTable("Pension", "SO");

                entity.Property(e => e.EndDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Franked).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Growth).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Income).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.PensionRebootFromType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PensionType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ProductFees).HasColumnType("decimal(5, 4)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PensionDetails>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("PensionDetails", "SO");

                entity.Property(e => e.FromDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ToDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PensionDrawdown>(entity =>
            {
                entity.HasKey(e => e.Index);

                entity.ToTable("PensionDrawdown", "AS");

                entity.Property(e => e.Index).ValueGeneratedNever();

                entity.Property(e => e.MinRate).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<Platform>(entity =>
            {
                entity.ToTable("Platform", "PC");

                entity.Property(e => e.PlatformName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PlatformType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SubType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<PreservationAge>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("PreservationAge", "AS");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "PC");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ProductType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.SubType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductFees>(entity =>
            {
                entity.HasKey(e => e.FeeId);

                entity.ToTable("ProductFees", "PC");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CostType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FeeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FeeType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.HeaderType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductFund>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("ProductFund", "PC");

                entity.Property(e => e.Apircode)
                    .IsRequired()
                    .HasColumnName("APIRCode")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FeeLabel1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FeeLabel2)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FeeLabel3)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductReplacement>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("ProductReplacement", "PC");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("Property", "SO");

                entity.Property(e => e.EndDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Growth).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.StartDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProposedClientFunds>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("ProposedClientFunds", "PC");

                entity.Property(e => e.Apircode)
                    .IsRequired()
                    .HasColumnName("APIRCode")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Value).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<ProposedClientProducts>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("ProposedClientProducts", "PC");

                entity.Property(e => e.RecId).ValueGeneratedNever();

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Value).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<ProposedFeeDetails>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("ProposedFeeDetails", "IN");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.FeeType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Frequency)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SpecialNotes).IsUnicode(false);
            });

            modelBuilder.Entity<ProposedIncomeCover>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("ProposedIncomeCover", "IN");

                entity.Property(e => e.BenefitPeriod)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Definition)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MonthlyBenefitAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PolicyOwner)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PremiumType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WaitingPeriod)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProposedInsuranceProducts>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("ProposedInsuranceProducts", "IN");

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Provider)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProposedLifeCover>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("ProposedLifeCover", "IN");

                entity.Property(e => e.BenefitAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PolicyOwner)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PremiumType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProposedTpdCover>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("ProposedTpdCover", "IN");

                entity.Property(e => e.BenefitAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Definition)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DisabilityTerm)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PolicyOwner)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PremiumType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StandaloneOrLinked)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WithinSuper)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProposedTraumaCover>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("ProposedTraumaCover", "IN");

                entity.Property(e => e.BenefitAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PolicyOwner)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PremiumType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StandaloneOrLinked)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WithinSuper)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<QualifyingAge>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("QualifyingAge", "AS");

                entity.Property(e => e.Age).HasColumnType("decimal(5, 1)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<RiskProfile>(entity =>
            {
                entity.HasKey(e => e.RiskProfile1);

                entity.ToTable("RiskProfile", "PC");

                entity.Property(e => e.RiskProfile1)
                    .HasColumnName("RiskProfile")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Defensive).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DefensiveAlternatives).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DefensiveAlternativesMax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DefensiveAlternativesMin).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DefensiveMax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DefensiveMin).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticCash).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticCashMax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticCashMin).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticEquity).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticEquityMax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticEquityMin).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticFixedInterest).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticFixedInterestMax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticFixedInterestMin).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticProperty).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticPropertyMax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DomesticPropertyMin).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Growth).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.GrowthAlternatives).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.GrowthAlternativesMax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.GrowthAlternativesMin).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.GrowthMax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.GrowthMin).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalCash).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalCashMax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalCashMin).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalEquity).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalEquityMax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalEquityMin).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalFixedInterest).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalFixedInterestMax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalFixedInterestMin).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalProperty).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalPropertyMax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InternationalPropertyMin).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.OtherGrowth).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.OtherGrowthMax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.OtherGrowthMin).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<RopcurrentFunds>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("ROPCurrentFunds", "PC");

                entity.Property(e => e.Apircode)
                    .IsRequired()
                    .HasColumnName("APIRCode")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Value).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<RopcurrentProducts>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("ROPCurrentProducts", "PC");

                entity.Property(e => e.RecId).ValueGeneratedNever();

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Value).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Sgcrate>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("SGCRate", "AS");

                entity.Property(e => e.Sgcrate1)
                    .HasColumnName("SGCRate")
                    .HasColumnType("decimal(5, 1)");
            });

            modelBuilder.Entity<Super>(entity =>
            {
                entity.ToTable("Super", "SO");

                entity.Property(e => e.EndDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Franked).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Growth).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Income).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.IncreaseToLimit)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.ProductFees).HasColumnType("decimal(5, 4)");

                entity.Property(e => e.Sgrate)
                    .HasColumnName("SGRate")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.StartDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SuperAssumptions>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("SuperAssumptions", "AS");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SuperDetails>(entity =>
            {
                entity.HasKey(e => e.RecId);

                entity.ToTable("SuperDetails", "SO");

                entity.Property(e => e.FromDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.IncreaseToLimit)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ToDateType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });
        }
    }
}
