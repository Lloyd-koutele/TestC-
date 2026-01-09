using BanqueKOUT;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BanqueTest
{
    [TestClass]
    public sealed class CompteBancaireTests
    {
        [TestMethod]
        public void VerifierDebitCompte()
        {
            // Arrange
            const double soldeInitial = 500000;
            const double montantDebit = 400000;
            const double soldeAttendu = 100000;
            var compte = new CompteBancaire("Pr Ibrahim Fall", soldeInitial);

            // Act
            compte.Debiter(montantDebit);

            // Assert
            Assert.AreEqual(soldeAttendu, compte.Solde, 0.001, "Compte débité incorrectement");
        }

        [TestMethod]
        public void DebitMontantNegatifSouleveArgumentOutOfRangeException()
        {
            // Arrange
            const double soldeInitial = 500000;
            const double montantDebit = -400000;
            var compte = new CompteBancaire("Pr Ibrahima NGOM", soldeInitial);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => compte.Debiter(montantDebit));
            StringAssert.Contains(ex.Message, "Le montant debite doit etre positif");
        }

        [TestMethod]
        public void DebitImpossibleCompteBloqueSouleveException()
        {
            // Arrange
            const double soldeInitial = 500000;
            const double montantDebit = 400000;
            var compte = new CompteBancaire("Pr Cheikh Anta DIOP", soldeInitial);

            // Simuler le blocage du compte (en utilisant la réflexion pour accéder au champ privé)
            var field = typeof(CompteBancaire).GetField("m_bloque", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(compte, true);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => compte.Debiter(montantDebit));
            StringAssert.Contains(ex.Message, "Compte bloqué");
        }

        [TestMethod]
        public void DebitMontantSuperieurAuSoldeSouleveExeption()
        {
            // Arrange
            const double soldeInitial = 500000;
            const double montantDebit = 600000;
            var compte = new CompteBancaire("Pr Alpha Blondy", soldeInitial);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => compte.Debiter(montantDebit));
            StringAssert.Contains(ex.Message, "Montant debite doit etre inferieur ou egal au solde disponible");
        }

        [TestMethod]
        public void CrediterCompte()
        {
            // Arrange
            const double soldeInitial = 500000;
            const double montantCredit = 200000;
            const double soldeAttendu = 700000;
            var compte = new CompteBancaire("Pr Youssou N'Dour", soldeInitial);
            // Act
            compte.Crediter(montantCredit);
            // Assert
            Assert.AreEqual(soldeAttendu, compte.Solde, 0.001, "Compte crédité incorrectement");
        }

        [TestMethod]
        public void CreditMontantNegatifSouleveArgumentOutOfRangeException()
        {
            // Arrange
            const double soldeInitial = 500000;
            const double montantCredit = -200000;
            var compte = new CompteBancaire("Pr Baaba Maal", soldeInitial);
            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => compte.Crediter(montantCredit));
            StringAssert.Contains(ex.Message, "Le montant credite doit etre positif");
        }

        [TestMethod]
        public void CreditImpossibleCompteBloqueSouleveArgumentOutOfRangeException()
        {
            // Arrange
            const double soldeInitial = 500000;
            const double montantCredit = 200000;
            var compte = new CompteBancaire("Pr Oumou Sangare", soldeInitial);

            // Simuler le blocage du compte
            var field = typeof(CompteBancaire).GetField("m_bloque", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(compte, true);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => compte.Crediter(montantCredit));
            StringAssert.Contains(ex.Message, "Le compte est bloqué");
        }

        [TestMethod]
        public void TestVirementEntreComptes()
        {
            // Arrange
            const double soldeInitialSource = 800000;
            const double soldeInitialDestination = 300000;
            const double montantVirement = 250000;
            const double soldeAttenduSource = 550000;
            const double soldeAttenduDestination = 550000;

            var compteSource = new CompteBancaire("Pr Salif Keita", soldeInitialSource, false);
            var compteDestination = new CompteBancaire("Pr Mariama Ba", soldeInitialDestination, false);

            // Act
            compteSource.Debiter(montantVirement);
            compteDestination.Crediter(montantVirement);

            // Assert
            Assert.AreEqual(soldeAttenduSource, compteSource.Solde, 0.001, "Solde du compte source incorrect après virement");
            Assert.AreEqual(soldeAttenduDestination, compteDestination.Solde, 0.001, "Solde du compte destination incorrect après virement");
        }

        [TestMethod]
        public void VirementImpossibleCompteSourceBloqueSouleveException()
        {
            // Arrange
            const double soldeInitialSource = 800000;
            const double soldeInitialDestination = 300000;
            const double montantVirement = 250000;

            var compteSource = new CompteBancaire("Pr Salif Keita", soldeInitialSource, true); // Compte source bloqué
            var compteDestination = new CompteBancaire("Pr Mariama Ba", soldeInitialDestination, false);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => compteSource.Debiter(montantVirement));
            StringAssert.Contains(ex.Message, "Compte bloqué");
        }

        [TestMethod]
        public void VirementImpossibleCompteDebiteurInsuffisant()
        {
            // Arrange
            const double soldeInitialSource = 200000;
            const double soldeInitialDestination = 300000;
            const double montantVirement = 250000;

            var compteSource = new CompteBancaire("Pr Salif Keita", soldeInitialSource, false);
            var compteDestination = new CompteBancaire("Pr Mariama Ba", soldeInitialDestination, false);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => compteSource.Debiter(montantVirement));
            StringAssert.Contains(ex.Message, "Montant debite doit etre inferieur ou egal au solde disponible");
        }
    }
}