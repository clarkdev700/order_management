using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OpticaVL.Models
{
    public class AppContext:DbContext
    {
        public DbSet<Assurance> Assurances { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<EntreeStock> EntreeStocks { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<LigneCommande> LigneCommandes { get; set; }
        public DbSet<LigneVente> LigneVentes { get; set; }
        public DbSet<Marque> Marques { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Payement> Payements { get; set; }
        //public DbSet<ModelDivers> ModelDivers { get; set; }
       // public DbSet<ModelMonture> ModelMontures { get; set; }
        public DbSet<ModelVerre> ModelVerres { get; set; }
        public DbSet<ReceptionCommande> ReceptionCommandes { get; set; }
        public DbSet<Vente> Ventes { get; set; }
        public DbSet<GammeVerre> GammeVerres { get; set; }
        public DbSet<FactureProforma> FactureProformas { get; set; }
        //public DbSet<GammeVerrePuissance> GammeVerrePuissances { get; set; }
        //public DbSet<Puissance> Puissances { get; set; }
        public DbSet<MVerre> MVerres { get; set; }
        public DbSet<AssuranceCommande> AssuranceCommandes { get; set; }
        public DbSet<AssuranceVente> AssuranceVentes { get; set; }
    }
}