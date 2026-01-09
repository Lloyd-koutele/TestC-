using System;

namespace BanqueKOUT
{
    public class CompteBancaire
    {
        private string nom;
        private double m_solde;
        private bool m_bloque = false;
        private double credite;

        public double Solde
        {
            get { return m_solde; }
        }

        public CompteBancaire(string nom, double soldeInitial)
        {
            this.nom = nom;
            m_solde = soldeInitial;
        }

        public CompteBancaire(string nom, double soldeInitial, bool m_bloque)
        {
            this.nom = nom;
            m_solde = soldeInitial;
            this.m_bloque = m_bloque;
        }

        public void Debiter(double montant)
        {
            if (m_bloque)
            {
                throw new InvalidOperationException("Compte bloqué");
            }

            if (montant < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(montant), "Le montant debite doit etre positif");
            }

            if (montant > m_solde)
            {
                throw new ArgumentOutOfRangeException(nameof(montant), "Montant debite doit etre inferieur ou egal au solde disponible");
            }

            m_solde -= montant;
        }

        public void Crediter(double montant)
        {
            if (montant < 0)
            {
                throw new ArgumentOutOfRangeException("Le montant credite doit etre positif");
            }
            if (m_bloque)
            {
                throw new ArgumentOutOfRangeException("Le compte est bloqué"); 
            }
            m_solde += montant;
        }

        public void VirementEntre2Compte(CompteBancaire debiteur, CompteBancaire crediteur, double montant)
        {
            if (debiteur == null || crediteur == null)
            {
                throw new ArgumentNullException("Compte manquant");
            }  

            if (debiteur.m_bloque)
            {
                throw new InvalidOperationException("Compte débiteur bloqué");
            }

            if (crediteur.m_bloque)
            {
                throw new InvalidOperationException("Compte créditeur bloqué");
            }

            debiteur.Debiter(montant);
            crediteur.Crediter(montant);

        }
    }
}