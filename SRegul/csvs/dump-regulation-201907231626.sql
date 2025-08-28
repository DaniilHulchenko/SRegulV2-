-- MySQL dump 10.13  Distrib 5.5.62, for Win64 (AMD64)
--
-- Host: localhost    Database: regulation
-- ------------------------------------------------------
-- Server version	5.5.5-10.3.12-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `adrfacturation`
--

DROP TABLE IF EXISTS `adrfacturation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `adrfacturation` (
  `Num_Appel` bigint(20) DEFAULT NULL,
  `Nom` varchar(100) DEFAULT NULL,
  `Prenom` varchar(100) DEFAULT NULL,
  `Num_Rue` varchar(5) DEFAULT NULL,
  `Adr1` varchar(250) DEFAULT NULL,
  `Adr2` varchar(250) DEFAULT NULL,
  `CodePostal` varchar(10) DEFAULT NULL,
  `Commune` varchar(100) DEFAULT NULL,
  `Pays` varchar(100) DEFAULT NULL,
  UNIQUE KEY `adrfacturation_Num_Appel_IDX` (`Num_Appel`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `adrfacturation`
--

LOCK TABLES `adrfacturation` WRITE;
/*!40000 ALTER TABLE `adrfacturation` DISABLE KEYS */;
INSERT INTO `adrfacturation` VALUES (3000001,'GUILBAUD','Lilou','5',' Epace  BOZON','','01200','Leaz','Suisse');
/*!40000 ALTER TABLE `adrfacturation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `appels`
--

DROP TABLE IF EXISTS `appels`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `appels` (
  `Num_Appel` bigint(20) NOT NULL,
  `Tel_Appel` varchar(30) DEFAULT NULL,
  `Num_Personne` bigint(20) DEFAULT NULL,
  `Nom` varchar(100) DEFAULT NULL,
  `Prenom` varchar(100) DEFAULT NULL,
  `Num_Rue` varchar(5) DEFAULT NULL,
  `Adr1` varchar(250) DEFAULT NULL,
  `Adr2` varchar(250) DEFAULT NULL,
  `CodePostal` varchar(10) DEFAULT NULL,
  `Commune` varchar(100) DEFAULT NULL,
  `Pays` varchar(100) DEFAULT NULL,
  `Email` varchar(150) DEFAULT NULL,
  `Digicode` varchar(20) DEFAULT NULL,
  `InterphoneNom` varchar(100) DEFAULT NULL,
  `Etage` varchar(20) DEFAULT NULL,
  `Porte` varchar(14) DEFAULT NULL,
  `Sexe` varchar(5) DEFAULT NULL,
  `DateNaissance` date DEFAULT NULL,
  `Provenance` varchar(30) DEFAULT NULL,
  `IdMotif1` varchar(5) DEFAULT NULL,
  `IdMotif2` varchar(5) DEFAULT NULL,
  `Commentaire` varchar(250) DEFAULT NULL,
  `Urgence` int(11) DEFAULT NULL,
  `CodeMedecin` int(11) DEFAULT -1,
  `Termine` tinyint(1) DEFAULT NULL,
  `IdMotifAnnulation` varchar(5) DEFAULT NULL,
  `TA` tinyint(1) DEFAULT NULL,
  `Remarquable` tinyint(1) DEFAULT NULL,
  UNIQUE KEY `appels_num_appels1_idx` (`Num_Appel`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Table de stockage des appels';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appels`
--

LOCK TABLES `appels` WRITE;
/*!40000 ALTER TABLE `appels` DISABLE KEYS */;
INSERT INTO `appels` VALUES (3000000,'+41227484950',-1,'SOS Médecins','toto ','43','Rue Louis-Favre','SOS Médecins','1201','Genève','Suisse ','','','','','','F','1970-01-01','PRIVE-PARTICULIER','A29','','',0,-1,0,'',0,0),(3000001,'+41227484950',-1,'GUILBAUD','Lilou ','43','Rue Louis-Favre','SOS Médecins','1201','Genève','Suisse ','','','','','','F','2016-12-20','PRIVE-PARTICULIER','5087','32','tttttttt',0,2887,0,'',0,0),(3000002,'+41227484950',275554,'SAUTAUX','Bernard ','68','  Communes Reunies','','1212','Grand-Lancy','Suisse ','','','','1','','H','1951-06-01','PRIVE-PARTICULIER','','','',0,1401,0,'',0,0),(3000003,'+41227484950',269489,'KHADRAOUI','Damas ','30','Rue des Charmilles','','1203','Genève','Suisse ','','69 13','','3','asc a droite -','H','2017-02-13','PRIVE-PARTICULIER','9535','','',0,-1,0,'',0,0);
/*!40000 ALTER TABLE `appels` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `appelsencours`
--

DROP TABLE IF EXISTS `appelsencours`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `appelsencours` (
  `Num_Appel` bigint(20) NOT NULL,
  `Tel_Appel` varchar(30) DEFAULT NULL,
  `Num_Personne` bigint(20) DEFAULT NULL,
  `Nom` varchar(100) DEFAULT NULL,
  `Prenom` varchar(100) DEFAULT NULL,
  `Num_Rue` varchar(5) DEFAULT NULL,
  `Adr1` varchar(250) DEFAULT NULL,
  `Adr2` varchar(250) DEFAULT NULL,
  `CodePostal` varchar(10) DEFAULT NULL,
  `Commune` varchar(100) DEFAULT NULL,
  `Pays` varchar(100) DEFAULT NULL,
  `Email` varchar(150) DEFAULT NULL,
  `Digicode` varchar(20) DEFAULT NULL,
  `InterphoneNom` varchar(100) DEFAULT NULL,
  `Etage` varchar(20) DEFAULT NULL,
  `Porte` varchar(20) DEFAULT NULL,
  `Sexe` varchar(5) DEFAULT NULL,
  `DateNaissance` date DEFAULT NULL,
  `Provenance` varchar(30) DEFAULT NULL,
  `Motif1` varchar(150) DEFAULT NULL,
  `Motif2` varchar(150) DEFAULT NULL,
  `Commentaire` varchar(250) DEFAULT NULL,
  `Urgence` int(11) DEFAULT NULL,
  `CodeMedecin` int(11) DEFAULT -1,
  UNIQUE KEY `appelsencours_num_appels2_idx` (`Num_Appel`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Table de stockage des appels';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appelsencours`
--

LOCK TABLES `appelsencours` WRITE;
/*!40000 ALTER TABLE `appelsencours` DISABLE KEYS */;
INSERT INTO `appelsencours` VALUES (3000000,'+41227484950',-1,'SOS Médecins','toto ','43','Rue Louis-Favre','SOS Médecins','1201','Genève','Suisse ','','','','','','F','1970-01-01','PRIVE-PARTICULIER','TEST INFORMATIQUE','','',0,-1),(3000001,'+41227484950',-1,'GUILBAUD','Lilou ','43','Rue Louis-Favre','SOS Médecins','1201','Genève','Suisse ','','','','','','F','2016-12-20','PRIVE-PARTICULIER','ALCOOL (AIGU)','ALTERATION ETAT GENERAL','tttttttt',0,2887),(3000002,'+41227484950',275554,'SAUTAUX','Bernard ','68','  Communes Reunies','','1212','Grand-Lancy','Suisse ','','','','1','','H','1951-06-01','PRIVE-PARTICULIER','DECHIRURE MUSCULAIRE','','',0,1401),(3000003,'+41227484950',269489,'KHADRAOUI','Damas ','30','Rue des Charmilles','','1203','Genève','Suisse ','','69 13','','3','asc a droite -','H','2017-02-13','PRIVE-PARTICULIER','DORSALGIE','','',0,-1);
/*!40000 ALTER TABLE `appelsencours` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `commune`
--

DROP TABLE IF EXISTS `commune`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `commune` (
  `IdCommune` int(11) NOT NULL AUTO_INCREMENT,
  `NomCommune` varchar(150) DEFAULT NULL,
  `CodePostal` varchar(6) DEFAULT NULL,
  PRIMARY KEY (`IdCommune`)
) ENGINE=InnoDB AUTO_INCREMENT=541 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `commune`
--

LOCK TABLES `commune` WRITE;
/*!40000 ALTER TABLE `commune` DISABLE KEYS */;
INSERT INTO `commune` VALUES (10,'AÏRE','1219'),(11,'AÏRE-LA-VILLE','1288'),(15,'ALLINGES','74200'),(16,'AMBILLY','74100'),(19,'ANIERES','1247'),(21,'ANNEMASSE','74100'),(22,'ANTHY-SUR-LÉMAN','74200'),(24,'ARCHAMPS','74160'),(25,'ARZIER','1273'),(26,'ATHENAZ','1285'),(30,'AVULLY','1237'),(31,'AVUSY','1285'),(35,'BAGNES','1936'),(36,'BARDONNEX','1257'),(37,'BASSINS','1269'),(38,'BEAUMONT','74160'),(39,'BEGNINS','1268'),(42,'BELLEVUE','1293'),(45,'BERNEX','1233'),(55,'BOGIS-BOSSEY','1279'),(56,'BOREX','1277'),(57,'BOSSEY','74160'),(58,'BULLE ','1630'),(59,'BURSINEL','1195'),(67,'CAROUGE','1227'),(81,'CARTIGNY','1236'),(82,'CELIGNY','1298'),(83,'CESSY','01170'),(84,'CHALLEX','01630'),(85,'CHAMBÉSY','1292'),(86,'CHAMPEL','1206'),(88,'CHANCY','1284'),(91,'CHATELAINE','1219'),(96,'CHAVANNES-DE-BOGIS ','1279'),(97,'CHAVANNES-DES-BOIS ','1290'),(104,'CHENE-BOUGERIES','1224'),(113,'CHENE-BOURG','1925'),(122,'CHENS-SUR-LÉMAN','74140'),(123,'CHEVRY','01170'),(127,'CHOULEX','1244'),(129,'COINTRIN','1216'),(131,'COLLEX','1239'),(134,'COLLEX-BOSSY','1239'),(143,'COLLONGE-BELLERIVE','1245'),(144,'COLLONGES SOUS SALÈVE','74160'),(155,'COLOGNY','1223'),(157,'COMMUGNY','1291'),(158,'CONCHES','1231'),(163,'CONFIGNON','1232'),(166,'COPPET ','1296'),(167,'CORSEAUX','1802'),(168,'CORSIER','1246'),(174,'CRANS-PRÈS-CÉLIGNY ','1299'),(175,'CRASSIER ','1263'),(176,'CRISSIER','1023'),(177,'LA CROIX-DE-ROZON','1257'),(180,'CROZET-AVOUZON','1170'),(182,'DARDAGNY','1283'),(184,'DIVONNE-LES-BAINS','01220'),(185,'DUILLIER','1266'),(186,'ECUBLENS','1024'),(187,'ETREMBIÈRES','74100'),(188,'EYSINS','1262'),(189,'FERNAY-VOLTAIRE','01210'),(191,'FOUNEX','1297'),(197,'GAILLARD ','74240'),(198,'GENEVE','1201'),(199,'GENEVE','1202'),(200,'GENEVE','1203'),(201,'GENEVE','1204'),(202,'GENEVE','1205'),(203,'GENEVE','1206'),(204,'GENEVE','1207'),(205,'GENEVE','1208'),(206,'GENEVE','1209'),(277,'GENÈVE AÉROPORT','1215'),(278,'GENOLIER','1272'),(282,'GENTHOD','1294'),(285,'GINGINS','1276'),(286,'GIVRINS','1271'),(287,'GLAND','1196'),(289,'GRAND SACONNEX','1218'),(291,'GRAND-LANCY','1212'),(292,'GRAND-SACONNEX','1218'),(293,'GRENS ','1274'),(294,'GY','1251'),(298,'HERMANCE','1248'),(299,'JOUXTENS-MEZERY','1008'),(301,'JUSSY','1254'),(306,'LA PLAINE','1283'),(307,'LA RIPPE ','1278'),(308,'LACONNEX','1287'),(312,'LANCY','1213'),(313,'LANCY','1223'),(314,'LANCY','1213'),(315,'LANCY','1217'),(316,'LANCY','1220'),(317,'LANCY','1219'),(318,'LANCY','1227'),(319,'LANCY','1214'),(320,'LANCY','1226'),(321,'LANCY','1205'),(322,'LANCY','12'),(323,'LAUSANNE','1005'),(324,'LAUSANNE','1007'),(325,'LAUSANNE','1003'),(326,'LAUSANNE','1006'),(327,'LAVIGNY','1175'),(328,'LE AVANCHETS','1220'),(333,'LE GRAND-SACONNEX','1218'),(340,'LE LIGNON','1219'),(343,'LE PAS DE L\'ECHELLE','74100'),(346,'LE VAUD ','1261'),(347,'LES ACACIAS','1227'),(349,'LES AVANCHETS','1220'),(352,'LUINS','1184'),(353,'LULLY','1233'),(355,'MEINIER','1252'),(358,'MEYLAN','38240'),(369,'MEYRIN','1217'),(370,'MIES','1295'),(371,'MORGES','1110'),(373,'NEYDENS','74160'),(375,'NYON ','1260'),(378,'ONEX','1213'),(387,'ORBE','1350'),(388,'ORNEX','01210'),(390,'PERLY','1258'),(393,'PERON','01630'),(399,'PETIT-LANCY','1213'),(403,'PLAN-LES-OUATES','1128'),(412,'POUGNY','01550'),(413,'PRANGINS ','1197'),(424,'PRESINGE','1243'),(425,'PRÉVESSIN-MOËNS','01280'),(427,'PRILLY','1008'),(429,'PUPLINGE','1241'),(433,'RENENS','1020'),(434,'RUSSIN','1281'),(435,'SAINT CERGUE','1264'),(437,'SAINT GENIS POUILLY','01630'),(438,'SAINT-JULIEN-EN-GENEVOIS','74160'),(442,'SATIGNY','1242'),(443,'SAUVERNY ','01220'),(444,'SEGNY','01170'),(445,'SERGY','01630'),(446,'SIERRE','3960'),(447,'SIGNY ','1274'),(448,'SION','1950'),(449,'SORAL','1286'),(451,'TANNAY','1295'),(453,'THOIRY','01710'),(461,'THONEX','1226'),(467,'TRELEX','1270'),(468,'TROINEX','1256'),(470,'VALLEIRY','74520'),(471,'VANDOEUVRES','1253'),(479,'VEIGY','74580'),(480,'VEIGY-FONCENEX ','74140'),(481,'VERNIER','1214'),(508,'VERSOIX','1290'),(519,'VERSONNEX ','01210'),(520,'VÉSENAZ','1222'),(523,'VESSY','1234'),(535,'VEYRIER','1255'),(536,'VIRY','74580');
/*!40000 ALTER TABLE `commune` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `engarde`
--

DROP TABLE IF EXISTS `engarde`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `engarde` (
  `IdStatusGarde` bigint(20) NOT NULL AUTO_INCREMENT,
  `IdGarde` int(11) DEFAULT NULL,
  `CodeMedecin` int(11) DEFAULT NULL,
  `IdSmartphone` int(11) DEFAULT NULL,
  `StatusGarde` varchar(20) DEFAULT NULL,
  `Nb_Visite` int(11) DEFAULT 0,
  `DateDebGarde` datetime DEFAULT NULL,
  UNIQUE KEY `engarde_IdStatusGarde_IDX` (`IdStatusGarde`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COMMENT='\r\nTable de travail, permettant de savoir où en est le médecin dans la visite: Disponible, Attribuée, Acquitée, debut de visite\r\n(D, AT, AQ, V)\r\nPour chaque changement, écrire une ligne dans SuiviAppel, \r\nQuand la visite est terminée, l''enregistrement est supprimé.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `engarde`
--

LOCK TABLES `engarde` WRITE;
/*!40000 ALTER TABLE `engarde` DISABLE KEYS */;
INSERT INTO `engarde` VALUES (1,4,1401,1,'Disponible',1,'2019-07-03 13:08:06'),(2,3,2887,2,'Attribuée',1,'2019-07-03 16:03:43');
/*!40000 ALTER TABLE `engarde` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `evenements`
--

DROP TABLE IF EXISTS `evenements`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `evenements` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `DateEvenement` datetime DEFAULT NULL,
  `Evenement` varchar(250) DEFAULT NULL,
  `Utilisateur` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `evenements_Id_IDX` (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=86 DEFAULT CHARSET=utf8 COMMENT='Table du mouchard';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `evenements`
--

LOCK TABLES `evenements` WRITE;
/*!40000 ALTER TABLE `evenements` DISABLE KEYS */;
INSERT INTO `evenements` VALUES (1,'2019-06-07 15:21:10','Connexion utilisateur','MERCIER Dominique'),(2,'2019-06-12 14:24:39','Connexion utilisateur','MERCIER Dominique'),(3,'2019-06-12 14:24:39','Prise du dispatch','Dominique MERCIER'),(4,'2019-06-12 14:25:44','Connexion utilisateur','MERCIER Dominique'),(5,'2019-06-12 14:25:44','Prise du dispatch','Dominique MERCIER'),(6,'2019-06-12 14:28:19','Connexion utilisateur','MERCIER Dominique'),(7,'2019-06-12 14:28:19','Prise du dispatch','Dominique MERCIER'),(8,'2019-06-12 14:30:11','Connexion utilisateur','MERCIER Dominique'),(9,'2019-06-12 14:30:31','Connexion utilisateur','MERCIER Dominique'),(10,'2019-06-12 14:43:29','Connexion utilisateur','MERCIER Dominique'),(11,'2019-06-12 14:44:06','Connexion utilisateur','MERCIER Dominique'),(12,'2019-06-12 14:44:08','Prise du dispatch','Dominique MERCIER'),(13,'2019-06-12 14:46:35','Connexion utilisateur','MERCIER Dominique'),(14,'2019-06-12 14:47:03','Prise du dispatch','Dominique MERCIER'),(15,'2019-06-12 14:50:29','Connexion utilisateur','MERCIER Dominique'),(16,'2019-06-12 14:50:41','Prise du dispatch','Dominique MERCIER'),(17,'2019-06-12 14:51:20','Connexion utilisateur','MERCIER Dominique'),(18,'2019-06-12 14:51:24','Prise du dispatch','Dominique MERCIER'),(19,'2019-06-14 12:07:40','Remise du dispatch','MERCIER Dominique'),(20,'2019-06-14 12:22:03','Remise du dispatch','MERCIER Dominique'),(21,'2019-06-14 12:28:36','Remise du dispatch','MERCIER Dominique'),(22,'2019-06-14 12:29:55','Remise du dispatch','MERCIER Dominique'),(23,'2019-06-14 12:32:31','Remise du dispatch','MERCIER Dominique'),(24,'2019-06-14 12:34:22','Remise du dispatch','MERCIER Dominique'),(25,'2019-06-14 12:36:48','Remise du dispatch','MERCIER Dominique'),(26,'2019-06-18 16:22:27','Remise du dispatch','MERCIER Dominique'),(27,'2019-06-18 16:26:48','Remise du dispatch','MERCIER Dominique'),(28,'2019-06-18 16:30:32','Remise du dispatch','MERCIER Dominique'),(29,'2019-06-18 16:38:05','Remise du dispatch','MERCIER Dominique'),(30,'2019-06-18 16:40:26','Remise du dispatch','MERCIER Dominique'),(31,'2019-06-18 16:42:23','Remise du dispatch','MERCIER Dominique'),(32,'2019-06-18 16:51:45','Remise du dispatch','MERCIER Dominique'),(33,'2019-06-18 16:54:15','Remise du dispatch','MERCIER Dominique'),(34,'2019-06-18 17:09:30','Remise du dispatch','MERCIER Dominique'),(35,'2019-06-18 17:12:27','Remise du dispatch','MERCIER Dominique'),(36,'2019-06-18 17:16:48','Remise du dispatch','MERCIER Dominique'),(37,'2019-06-18 17:29:34','Remise du dispatch','MERCIER Dominique'),(38,'2019-06-18 17:31:08','Remise du dispatch','MERCIER Dominique'),(39,'2019-06-18 17:33:08','Remise du dispatch','MERCIER Dominique'),(40,'2019-06-18 17:42:27','Remise du dispatch','MERCIER Dominique'),(41,'2019-06-18 17:59:15','Remise du dispatch','MERCIER Dominique'),(42,'2019-06-19 11:05:55','Remise du dispatch','MERCIER Dominique'),(43,'2019-06-19 16:14:08','Remise du dispatch','MERCIER Dominique'),(44,'2019-06-19 16:19:18','Remise du dispatch','MERCIER Dominique'),(45,'2019-06-19 17:37:34','Remise du dispatch',' '),(46,'2019-06-19 17:39:04','Remise du dispatch','MERCIER Dominique'),(47,'2019-06-19 17:40:36','Remise du dispatch','MERCIER Dominique'),(48,'2019-06-19 17:49:00','Remise du dispatch','MERCIER Dominique'),(49,'2019-06-19 17:49:48','Remise du dispatch','MERCIER Dominique'),(50,'2019-06-19 17:53:35','Remise du dispatch','MERCIER Dominique'),(51,'2019-06-19 17:54:32','Remise du dispatch','MERCIER Dominique'),(52,'2019-06-19 17:55:09','Remise du dispatch','MERCIER Dominique'),(53,'2019-06-19 17:55:42','Remise du dispatch','MERCIER Dominique'),(54,'2019-06-19 17:56:50','Remise du dispatch','MERCIER Dominique'),(55,'2019-06-20 12:51:41','Remise du dispatch','MERCIER Dominique'),(56,'2019-06-21 10:33:35','Remise du dispatch','MERCIER Dominique'),(57,'2019-06-21 14:45:56','Remise du dispatch','MERCIER Dominique'),(58,'2019-06-21 14:50:49','Remise du dispatch','MERCIER Dominique'),(59,'2019-06-21 15:01:26','Remise du dispatch','MERCIER Dominique'),(60,'2019-06-21 15:16:08','Remise du dispatch','MERCIER Dominique'),(61,'2019-06-21 16:24:26','Remise du dispatch','MERCIER Dominique'),(62,'2019-06-21 16:25:42','Remise du dispatch','MERCIER Dominique'),(63,'2019-06-21 16:29:59','Remise du dispatch','MERCIER Dominique'),(64,'2019-06-21 16:47:15','Remise du dispatch','MERCIER Dominique'),(65,'2019-06-21 16:49:29','Remise du dispatch','MERCIER Dominique'),(66,'2019-06-21 16:50:19','Remise du dispatch','MERCIER Dominique'),(67,'2019-06-27 15:20:37','Remise du dispatch','MERCIER Dominique'),(68,'2019-06-27 15:24:12','Remise du dispatch','MERCIER Dominique'),(69,'2019-06-27 15:48:15','Remise du dispatch','MERCIER Dominique'),(70,'2019-06-27 15:57:58','Remise du dispatch','MERCIER Dominique'),(71,'2019-06-27 16:03:18','Remise du dispatch',' '),(72,'2019-06-27 16:20:58','Remise du dispatch','MERCIER Dominique'),(73,'2019-06-27 16:31:27','Remise du dispatch','MERCIER Dominique'),(74,'2019-06-28 12:28:28','Remise du dispatch','MERCIER Dominique'),(75,'2019-07-02 15:35:27','Remise du dispatch','MERCIER Dominique'),(76,'2019-07-03 14:17:56','Remise du dispatch','MERCIER Dominique'),(77,'2019-07-03 15:09:49','Remise du dispatch','MERCIER Dominique'),(78,'2019-07-03 15:28:01','Remise du dispatch','MERCIER Dominique'),(79,'2019-07-03 16:06:40','Remise du dispatch','MERCIER Dominique'),(80,'2019-07-03 17:04:10','Remise du dispatch','MERCIER Dominique'),(81,'2019-07-03 17:25:25','Remise du dispatch','MERCIER Dominique'),(82,'2019-07-03 17:42:34','Remise du dispatch','MERCIER Dominique'),(83,'2019-07-03 17:51:25','Remise du dispatch','MERCIER Dominique'),(84,'2019-07-03 17:54:18','Remise du dispatch','MERCIER Dominique'),(85,'2019-07-03 17:56:15','Remise du dispatch','MERCIER Dominique');
/*!40000 ALTER TABLE `evenements` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `garde`
--

DROP TABLE IF EXISTS `garde`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `garde` (
  `IdGarde` int(11) NOT NULL AUTO_INCREMENT,
  `TypeGarde` varchar(5) DEFAULT NULL,
  `HeureDeb` varchar(5) DEFAULT NULL,
  `HeureFin` varchar(5) DEFAULT NULL,
  UNIQUE KEY `garde_IdGarde_IDX` (`IdGarde`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8 COMMENT='Type de garde (J1, J2, etc...)\r\nTable de paramètres';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `garde`
--

LOCK TABLES `garde` WRITE;
/*!40000 ALTER TABLE `garde` DISABLE KEYS */;
INSERT INTO `garde` VALUES (1,'J1','07:00','16.00'),(2,'J2','07:00','16.00'),(3,'J3','08:00','17.00'),(4,'J4','08:00','17.00'),(5,'J6','09:00','18.00'),(6,'J8','09:00','14.00'),(7,'P1','10:00','21.00'),(8,'S1','16:00','23.00');
/*!40000 ALTER TABLE `garde` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `histogarde`
--

DROP TABLE IF EXISTS `histogarde`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `histogarde` (
  `IdHisto` bigint(20) NOT NULL AUTO_INCREMENT,
  `CodeMedecin` int(11) DEFAULT NULL,
  `IdSmartphone` int(11) DEFAULT NULL,
  `IdGarde` int(11) DEFAULT NULL,
  `DebGarde` datetime DEFAULT NULL,
  `FinGarde` datetime DEFAULT NULL,
  `TypeGarde` varchar(5) DEFAULT NULL,
  `Nb_Visite` int(11) DEFAULT 0,
  UNIQUE KEY `histogarde_IdHisto_IDX` (`IdHisto`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Historique des gardes pour un médecin\r\n(nvl enreg à chaque nvlle garde)\r\nA la fin de la garde du médecin on y ajoute le nombre de visite faites';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `histogarde`
--

LOCK TABLES `histogarde` WRITE;
/*!40000 ALTER TABLE `histogarde` DISABLE KEYS */;
/*!40000 ALTER TABLE `histogarde` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `medecins`
--

DROP TABLE IF EXISTS `medecins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `medecins` (
  `CodeMedecin` int(11) DEFAULT NULL,
  `Nom` varchar(100) DEFAULT NULL,
  `Prenom` varchar(100) DEFAULT NULL,
  `DateDebActiv` datetime DEFAULT NULL,
  `DateFinActiv` datetime DEFAULT NULL,
  UNIQUE KEY `medecins_CodeMedecin_IDX` (`CodeMedecin`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `medecins`
--

LOCK TABLES `medecins` WRITE;
/*!40000 ALTER TABLE `medecins` DISABLE KEYS */;
INSERT INTO `medecins` VALUES (1401,'MERCIER','Dominique','2019-01-01 00:00:00',NULL),(2886,'BONNET','Christophe','2016-01-01 00:00:00',NULL),(2887,'SIBON','Marc','2016-01-01 00:00:00',NULL),(2890,'DE SENARCLENS','Olivier','2016-01-01 00:00:00',NULL),(2892,'FONTANEL','Anne','2016-01-01 00:00:00',NULL),(2901,'DANILOSKI','Michel','2016-01-01 00:00:00',NULL),(2905,'HORVATH','Rémi','2016-01-01 00:00:00',NULL),(2906,'DELGADO','David','2016-01-01 00:00:00',NULL),(0,'AUCUN','','2019-01-01 00:00:00',NULL);
/*!40000 ALTER TABLE `medecins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `motif`
--

DROP TABLE IF EXISTS `motif`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `motif` (
  `IdMotif` varchar(5) NOT NULL,
  `LibelleMotif` varchar(150) DEFAULT NULL,
  `CodeMotif` varchar(10) DEFAULT NULL,
  `TypeMotif` varchar(10) DEFAULT NULL,
  UNIQUE KEY `motif_IdMotif_IDX` (`IdMotif`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Table contenant la liste des motifs de consultation\r\nLe type motif est: V (visite) ou A (Annulation)';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `motif`
--

LOCK TABLES `motif` WRITE;
/*!40000 ALTER TABLE `motif` DISABLE KEYS */;
INSERT INTO `motif` VALUES ('01','BOBO','01','V'),('02','DELAI','DLE','A'),('03','PRIS EN DOUBLE','PED','A'),('04','VA A L HOPITAL','VAH','A'),('05','MEDECIN SUR PLACE','MSP','A'),('1','VIROSE','02','V'),('10','DL GENERALISEE','A01','V'),('100','FRISSONS','A02','V'),('1001','RGO','D90','V'),('101','TREMBLEMENTS','A02','V'),('1010','SYNDROME ABDOMINAL','D01','V'),('1011','DL ABDOMINALE','D01','V'),('1012','DL ESTOMAC','D01','V'),('1013','COLITE','D93','V'),('1014','EPIGASTRALGIE','D01','V'),('1015','GASTRALGIES','D01','V'),('1016','APPENDICITE','03','V'),('1020','VOMISSEMENT','D10','V'),('1021','INTOXICATION ALIMENTAIRE','D10','V'),('1022','NAUSEE','D09','V'),('1023','HEPATITE','D72','V'),('1024','JAUNISSE','D72','V'),('1030','DIARRHEE','D11','V'),('1031','GASTRO ENTERITE','D10','V'),('1040','CONSTIPATION','D12','V'),('1050','VOMISSEMENT SANG','D14','V'),('1051','SANG DANS VOMISSEMENTS','D14','V'),('1052','HEMATEMESE','D14','V'),('1053','HERNIE','D89','V'),('1054','HERNIE HIATALE','D90','V'),('1055','CROHN','D94','V'),('1060','DL DENTAIRE','D19','V'),('1061','RAGE DE DENTS','D19','V'),('1062','POUSSEE DENTAIRE','D19','V'),('1070','HEMORRAGIE ANUS','D16','V'),('1071','RECTORRAGIE','D16','V'),('1072','SANG DANS LES SELLES','D16','V'),('1073','MELAENA SELLES NOIRES','D15','V'),('1080','HEMORROIDES','K96','V'),('1090','DIVERS GASTRO','D29','V'),('1091','DL ANUS','D29','V'),('1092','PANCREATITE','D29','V'),('1095','GASTRITE','D29','V'),('1098','COLIQUES','D29','V'),('11','DL MUSCULAIRES','L18','V'),('110','CERTIFICAT DE DECES','A96','V'),('1110','PILULE','W10','V'),('1111','PILULE DU LENDEMAIN','W10','V'),('1112','CONTRACEPTION','W10','V'),('1120','SAIGNEMENT GROSSESSE','W03','V'),('1121','METRORRAGIE SUR GROSSESSE','W03','V'),('1122','HEMORRAGIE GROSSESSE','W03','V'),('1130','PROBLEMEB POST PARTUM','W18','V'),('1131','DL SEINS(ALLAITE)','W20','V'),('1132','ALLAITEMENT(PROBLEME)','W19','V'),('1133','SAIGNEMENT APRES ACCOUCHEMENT','A10','V'),('1134','DL APRES ACCOUCHEMENT','D01','V'),('1135','METRORRAGIE APRES ACCOUCHEMENT','A10','V'),('1140','NAUSEES GROSSESSE','W05','V'),('1150','GROSSESSE ET DIVERS GROSSESSE','W78','V'),('118','118','118','A'),('12','COURBATURES','L18','V'),('120','CERTIFICAT  DE COUPS ET BLESSURES','Z25','V'),('121','CERTIFICATS DIVERS','Z62','V'),('1210','REGLES DOULOUREUSES','X02','V'),('1211','DL REGLES','X02','V'),('122','SECONDAIRE','A29','V'),('1220','METRORRAGIES','X08','V'),('1230','PROBLEME GYNECO','X29','V'),('1231','DIVERS GYNECO','X29','V'),('1232','INFECTION GYNECO','X29','V'),('1233','VAGINITE','X29','V'),('1234','DYSMENHORREES','X29','V'),('13','PANARI3','A01','V'),('131','GAV','Z09','V'),('1310','DL TESTICULE','Y02','V'),('1320','DIVERS GENITAL MASCULIN','Y29','V'),('1321','GONOCOCCIE','Y29','V'),('1322','CHAUDE PISSE','Y29','V'),('1323','MST','Y29','V'),('1324','DL SEXE (MASCULIN)','Y29','V'),('14','ANGINE','R76','V'),('140','CONSEIL TELEPHONIQUE','45','V'),('1410','COMA DIABETE','A07','V'),('1411','COMA HYPOGLYCEMIQUE','A07','V'),('1412','MALAISE CHEZ DIABETIQUE','A06','V'),('1413','DIABETE','T90','V'),('1414','ANOREXIE','T06','V'),('1415','MALAISE HYPOGLYCEMIQUE','A06','V'),('1417','HYPOGLYCEMIE','A06','V'),('1420','DIVERS ENDOCRINO','T29','V'),('1421','CRISE D\'ACETONE','T29','V'),('1422','ACETONE','T29','V'),('144','144','144','A'),('15','VARICELLE','A72','V'),('150','PRESCRIPTION MEDICALE','50','V'),('151','RENOUVELLEMENT ORDO','50','V'),('1510','DIFFICULTE A. URINER','U05','V'),('1511','RETENTION D\'URINE','U05','V'),('1512','PROSTATE (PROBLEME DE)','U05','V'),('152','ORDONNANCE','50','V'),('1520','BRULURE URINAIRE','U01','V'),('153','INJECTION','53','V'),('1530','INFECTION URINAIRE','U71','V'),('1531','CYSTITE','U71','V'),('1532','PYELONEPHRITE','U71','V'),('1540','SANG DS URINES','U06','V'),('1541','HEMATURIE','U06','V'),('1550','COLIQUE NEPHRETIQUE','U95','V'),('1560','DIVERS UROLOGIE','U29','V'),('1561','SONDE (PROBLEME DE)','U29','V'),('16','VACCIN','44','V'),('160','INTOXICATION MEDICAMENTEUSE','A85','V'),('161','EFFET SECONDAIRE MEDICAMENT','A85','V'),('17','RIEN','05','V'),('170','INTOX SUBST. NON MEDICAMENTEUSE','A86','V'),('180','CANCER','A79','V'),('190','SIDA','B90','V'),('191','RESULTATS ANALYSE','60','V'),('20','FIEVRE','A03','V'),('200','PLUSIEURS PERSONNES','Z29','V'),('201','SCARLATINE','02','V'),('2010','MAL A L\'OEIL','F01','V'),('2011','DL OEIL','F01','V'),('2020','OEIL ROUGE','F02','V'),('2021','CONJONCTIVITE','F02','V'),('2030','CORPS ETRANGER OEIL','F76','V'),('2031','POUSSIERE OEIL','F76','V'),('2040','DIVERS OPHT','F29','V'),('2041','TROUBLES DE LA VUE','F29','V'),('209','DECHIRURE MUSCULAIRE','A80','V'),('21','GRIPPE','A03','V'),('210','TRAUMATOLOGIE','A80','V'),('211','CHUTE','A80','V'),('212','FRACTURE','A80','V'),('213','FRACTURE POIGNET','A80','V'),('214','FRACT MAIN','A80','V'),('215','FRACT JAMBE','A80','V'),('216','FRACT COTE','A80','V'),('217','FRACT PIED','A80','V'),('218','FRACT OS DU NEZ','A80','V'),('219','TENDINITE','A80','V'),('22','PALUDISME','A73','V'),('220','GANGLIONS','B02','V'),('221','FAUX MOUVEMENT','A80','V'),('2215','NE VEUT PLUS LE MEDECIN','PLM','A'),('2216','CONSEIL TELEPHONIQUE','CT','A'),('2218','PRIS EN DOUBLE','PED','A'),('2219','MEDECIN TRAITANT','MDT','A'),('222','DL NUQUE','L01','V'),('2220','ADRESSE INTROUVABLE','ADI','A'),('2221','PERSONNE SUR PLACE','PSP','A'),('2222','VA MIEUX','VAM','A'),('2223','NE REPOND PAS','NRP','A'),('2224','ENDORMI','EDM','A'),('230','SANS MOTIF','A97','V'),('231','DIV GENERAL','A29','V'),('232','TEST INFORMATIQUE','A29','V'),('233','EXAMEN CLINIQUE NORMAL','A29','V'),('25','FATA','FTA','A'),('26','DIVERS','DIV','A'),('30','FATIGUE','A04','V'),('3001','TRACHEITE','R29','V'),('3010','OTITE OTALGIE','H01','V'),('3011','DL OREILLE','H01','V'),('3021','DL GORGE','R21','V'),('3030','LARYNGITE','R77','V'),('3031','APHONE','R77','V'),('3040','CORPS ETRANGER ORL','R87','V'),('3041','ARETE DANS LA GORGE','R87','V'),('3050','RHINO-PHARYNGITE','R07','V'),('3051','RHINO-BRONCHITE','R07','V'),('3052','PHARYNGITE','R07','V'),('3060','EPISTAXIS','R06','V'),('3061','DIVERS ORL','R29','V'),('3062','SAIGNEMENT DE NEZ','R06','V'),('3063','SINUSITE','R75','V'),('3064','MAL A LA GORGE','R76','V'),('3065','PAROTIDITE','R29','V'),('31','ASTHENIE','A04','V'),('32','ALTERATION ETAT GENERAL','A05','V'),('33','SURMENAGE','A04','V'),('40','MALAISE CONSCIENT','A06','V'),('4001','INFARCTUS DU MYOCARDE','K01','V'),('4010','DL THORACIQUE','K01','V'),('4011','ANGINE DE POITRINE','K01','V'),('4012','PHLEBITE','k94','V'),('4015','PERICARDITE','K29','V'),('4020','DL THORACIQUE + ATCD','K01','V'),('4030','PALPITATIONS','K04','V'),('4031','TACHYCARDIE','K79','V'),('4032','ARYTHMIE','K78','V'),('4033','TACHY-ARYTHMIE','K78','V'),('4040','PROBLEME DE TENSION','K25','V'),('4041','TENSION','K25','V'),('4042','HYPERTENSION','K86','V'),('4043','HYPOTENSION','K88','V'),('4050','OEDEME DE JAMBE','K07','V'),('4051','JAMBES GONFLEE(S)','K07','V'),('4060','DIVERS CARDIO','K29','V'),('41','PC RC','A06','V'),('42','MALAISE VAGAL','A06','V'),('43','SYNCOPE CONSCIENT','A06','V'),('46','CONSULTATION','46','V'),('50','MALAISE INCONSCIENT','A07','V'),('5001','AMNESIE','P29','V'),('5010','AGITATION','P04','V'),('5011','DELIRE','P99','V'),('5020','CRISE DE NERFS','P02','V'),('5021','NERFS (CRISE DE)','P02','V'),('5022','CHOC EMOTIONNEL','P02','V'),('5023','CHOC REACTIONNEL','P02','V'),('5030','ANGOISSE','P01','V'),('5031','ANXIETE','P01','V'),('5040','DEPRESSION','P03','V'),('5050','TROUBLE DU SOMMEIL','P06','V'),('5051','SOMMEIL (VEUT DORMIR)','P06','V'),('5052','INSOMNIE','P06','V'),('5053','DORT MAL','P06','V'),('5060','TENTATIVE DE SUICIDE','P77','V'),('5061','TS','P77','V'),('5070','TETANIE','P75','V'),('5071','SPASMOPHILIE','P75','V'),('5080','ALCOOL (IVRE)','P16','V'),('5081','IVRESSE','P16','V'),('5082','CUITE','P16','V'),('5085','ALCOOL (CHRONIQUE)','P15','V'),('5086','SEVRAGE ALCOOLIQUE','P15','V'),('5087','ALCOOL (AIGU)','P16','V'),('5090','TOXICOMANIE','P19','V'),('5091','MANQUE (TOXICO)','P19','V'),('5092','OVERDOSE','P19','V'),('5093','ETAT DE MANQUE','P19','V'),('5094','ALZHEIMER','P70','V'),('5095','DIVERS PSYCHIATRIE','P29','V'),('5096','HDT','P29','V'),('5097','HYSTERIE','P29','V'),('52','SYNCOPE INCONSCIENT','A07','V'),('60','MALAISE+ATCD CARDIO','A06','V'),('70','MALAISE+ATCD DIABETE','A06','V'),('71','SAIGNEMENT','A10','V'),('80','ALLERGIE','A12','V'),('8010','DIFFICULTE RESPIRATOIRE','R02','V'),('8011','DYSPNEE','R02','V'),('8012','OAP','R02','V'),('8013','BRONCHIOLITE','R78','V'),('8014','BRONCHITE ASTHMATIFORME','R78','V'),('8015','BRONCHITE','R78','V'),('8016','INSUFF CARDIAQUE','R02','V'),('8017','INSUUF RESPIRATOIRE','R02','V'),('8020','TOUX','R05','V'),('8021','CYANOSE','S08','V'),('8030','ASTHME','R96','V'),('8040','DIVERS PNEUMO','R99','V'),('8041','DETRESSE RESPIRATOIRE','R99','V'),('81','URTICAIRE','S98','V'),('82','OEDEME DE QUINCKE','A12','V'),('90','PLEURE ENFANT','A15','V'),('9010','ERUPTIONS','S07','V'),('9011','BOUTONS','S07','V'),('9012','FESSES ROUGES(BEBE)','S07','V'),('9013','HERPES','S71','V'),('9014','ZONA','S70','V'),('9015','MYCOSE','S75','V'),('9016','MUGUET BUCCAL','S29','V'),('9017','CANDIDOSE BUCCALE','S29','V'),('9020','PLAIE SUTURE','S18','V'),('9021','PLAIE SANS SUTURE','S18','V'),('9030','BRULURE','S14','V'),('9040','DEMANGEAISON','S02','V'),('9041','PRURIT','S02','V'),('9050','PIQURE D\'INSECTE','S12','V'),('9060','MORSURE','S13','V'),('9070','CORP ETRANGER PEAU','S15','V'),('9079','FURONCLE','S10','V'),('9080','INFECTION PEAU','S76','V'),('9081','ABCES','S10','V'),('9083','PLAIE INFECTEE','S18','V'),('9084','SAIGNEMENT VARICES','A10','V'),('9090','DERMATO DIVERS','S29','V'),('9101','PERTE D\'EQUILIBRE','A06','V'),('9110','CEPHALEES','N01','V'),('9111','DL TETE','N01','V'),('9112','MAL DE TETE','N01','V'),('9120','CONVULSION FEBRILE (ENFANT)','A03','V'),('9130','EPILEPSIE','N07','V'),('9131','CONVULSION','N07','V'),('9200','MIGRAINE','N89','V'),('9210','VERTIGES','N17','V'),('9211','VERTIGE MENIERE','H82','V'),('9220','HEMIPLEGIE','N18','V'),('9221','PARALYSIE','N18','V'),('9222','AVC','N18','V'),('9223','ACCIDENT VASCULAIRE CEREBRAL','N18','V'),('9230','APHASIE','N19','V'),('9231','PARLE PLUS','N19','V'),('9232','TROUBLE PAROLE','N19','V'),('9240','DIVERS NEURO','N29','V'),('9241','SYNDROME MENINGE','A03','V'),('9510','DL BRAS','LMS','V'),('9511','COUDE','L10','V'),('9512','POIGNET','L11','V'),('9513','MAIN','L12','V'),('9514','HANCHE','L13','V'),('9515','JAMBE','L14','V'),('9516','GENOU','L15','V'),('9517','CHEVILLE','L16','V'),('9518','PIED','L17','V'),('9520','DL MEMBRE INFERIEUR','LMI','V'),('9521','DL MEMBRE SUPERIEUR','LMS','V'),('9522','DL INTERCOSTALE','L04','V'),('9523','DL MOLLET','LMI','V'),('9524','DL AINE','LMI','V'),('9530','DL DOS','L02','V'),('9531','LOMBALGIE','L03','V'),('9532','LUMBAGO','L03','V'),('9533','DOS BLOQUE','L02','V'),('9534','SCIATIQUE','L02','V'),('9535','DORSALGIE','L02','V'),('9536','MACHOIRE','L07','V'),('9537','EPAULE','L08','V'),('9538','BRAS','L09','V'),('9540','DIVERS RHUMATO','L29','V'),('9541','ARTHROSE','L29','V'),('9542','RHUMATISME','L29','V'),('9543','GOUTTE','L29','V'),('9544','ENTORSE','L79','V'),('9545','CERVICALGIE','L01','V'),('9546','TORTICOLIS','L01','V'),('9547','CRURALGIES','L29','V'),('9548','ALLERGIE,URTICAIRE,OEDEME...','AT14','V'),('9549','ACCOUCHEMENT EN COURS','AT25','V'),('9550','APPEL DU MEDECIN TRAITANT','AT29','V'),('9551','APPEL DU REGULATEUR SAMU-15','AT30','V'),('9552','APPELS CABINE','AT32','V'),('9553','APPELS \"LOUCHES\"','AT33','V'),('9554','ASTHME','ATI9','V'),('9555','BRULURES IMPORTANTES','AT28','V'),('9556','CONVULSION EPILEPSIE','ATI4','V'),('9557','DEPRESSIF SEUL AU DOMICILE','AT22','V'),('9558','DL ABDO CHEZ FEMME 15 A 50 ANS','AT26','V'),('9559','DL INTENSE (CN TESTICULE CANCER)','AT13','V'),('9560','DL POITRINE INFARCUS','AT12','V'),('9561','FIEVRE SUPERIEURE OU = A 40 °','AT27','V'),('9562','GENE RESP SPONTANEMENT SIGNALE','AT10','V'),('9563','HEMORRAGIE GENITALE REGLES+++','AT17','V'),('9564','INCONSCIENT NE REPOND PAS','ATI2','V'),('9565','MALAISE AVEC PC','ATI3','V'),('9566','MALAISE SANS PC','ATI6','V'),('9567','MOTIF DIFFICILE A PRECISER','AT34','V'),('9568','NE RESPIRE PLUS','ATI7','V'),('9569','PATIENT PANIQUE (OU ENTOURAGE)','ATI1','V'),('9570','PATIENT SIGNALE PAR SOS','AT31','V'),('9571','PLAIE SAIGNANT+++','AT18','V'),('9572','PRISE DE MEDICAMENT','AT21','V'),('9573','SAIGNE DU NEZ','AT19','V'),('9575','SUICIDE','AT20','V'),('9576','TOMBE NE SE RELEVE PAS','ATI5','V'),('9577','TOUT BLEU','ATI8','V'),('9578','TOUX SIFFLANTE-TOUX RAUQUE','AT11','V'),('9579','TOXICOMAN ALCOOL','AT24','V'),('9580','TROUBLE COMPORTEMENT AGITATION','AT23','V'),('9581','VOMIT DU SANG','AT15','V'),('9582','FIEVRE >= A 39° + SEJ ETRANGER','AT35','V'),('9583','FIEVRE INFERIEURE OU= A 36','AT36','V'),('9600','METADONE','META','V'),('9999','MAJORATION D\'URGENCE','9999','V'),('FATA','Fausse Alerte TA','FATA','V');
/*!40000 ALTER TABLE `motif` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `param_divers`
--

DROP TABLE IF EXISTS `param_divers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `param_divers` (
  `ActivationCTI` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `param_divers`
--

LOCK TABLES `param_divers` WRITE;
/*!40000 ALTER TABLE `param_divers` DISABLE KEYS */;
INSERT INTO `param_divers` VALUES (1);
/*!40000 ALTER TABLE `param_divers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `provenance`
--

DROP TABLE IF EXISTS `provenance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `provenance` (
  `IdProvenance` int(11) NOT NULL AUTO_INCREMENT,
  `LibelleProvenance` varchar(30) DEFAULT NULL,
  `CodeProvenance` varchar(5) DEFAULT NULL,
  PRIMARY KEY (`IdProvenance`)
) ENGINE=InnoDB AUTO_INCREMENT=121 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `provenance`
--

LOCK TABLES `provenance` WRITE;
/*!40000 ALTER TABLE `provenance` DISABLE KEYS */;
INSERT INTO `provenance` VALUES (1,'PRIVE-PARTICULIER','P'),(2,'HOTELS','H'),(3,'POLICE','117'),(4,'URGENCES SANTE','144'),(5,'Télé Alarme','TA'),(6,'POMPIERS','118'),(7,'MEDECIN TRAITANT','MT'),(8,'INST MED-SOC','EMS'),(9,'VOIE PUBLIQUE','VP'),(10,'ASSISTANCE INTERNATIONALE','AI');
/*!40000 ALTER TABLE `provenance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `smartphone`
--

DROP TABLE IF EXISTS `smartphone`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `smartphone` (
  `IdSmartphone` int(11) NOT NULL AUTO_INCREMENT,
  `NomSmartphone` varchar(100) DEFAULT NULL,
  `CarteSim` varchar(50) DEFAULT NULL,
  `NumTel` varchar(20) DEFAULT NULL,
  `CodeMedecin` int(11) DEFAULT NULL,
  `Actif` tinyint(1) DEFAULT NULL,
  UNIQUE KEY `smartphone_IdSmartphone_IDX` (`IdSmartphone`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 COMMENT='Gestion des smartphones';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `smartphone`
--

LOCK TABLES `smartphone` WRITE;
/*!40000 ALTER TABLE `smartphone` DISABLE KEYS */;
INSERT INTO `smartphone` VALUES (1,'Domi','12345','+41793095036',1401,1),(2,'Tournus10',NULL,'0797049128',NULL,1),(3,'Tournus11','555','+41793095023',0,1),(4,'Tournus12','321','+41793095011',0,1),(5,'Tournus13','321','+41793095041',0,1),(6,'Tournus14','123','+41793095014',0,1);
/*!40000 ALTER TABLE `smartphone` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `status_visite`
--

DROP TABLE IF EXISTS `status_visite`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `status_visite` (
  `IdStatusVisite` bigint(20) NOT NULL AUTO_INCREMENT,
  `Num_Appel` bigint(20) DEFAULT NULL,
  `CodeMedecin` int(11) DEFAULT -1,
  `Status` varchar(2) DEFAULT NULL,
  `DateStatus` datetime DEFAULT NULL,
  UNIQUE KEY `status_visite_IdStatusVisite_IDX` (`IdStatusVisite`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COMMENT='Table de travail, permettant de savoir où en est le médecin dans la visite: Non attribuée, Pré attribuée, Attribuée, Acquitée, debut de visite, terminée\r\n(NA, PR, AT, AQ, V, T)\r\nPour chaque changement, écrire une ligne dans SuiviAppel, \r\nQuand la visite est terminée, l''enregistrement est supprimé.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `status_visite`
--

LOCK TABLES `status_visite` WRITE;
/*!40000 ALTER TABLE `status_visite` DISABLE KEYS */;
INSERT INTO `status_visite` VALUES (1,3000000,-1,'NA','2019-07-19 17:16:16'),(2,3000001,2887,'AT','2019-07-15 17:17:02'),(3,3000002,1401,'PR','2019-07-19 17:30:38'),(4,3000003,-1,'NA','2019-07-19 17:30:52');
/*!40000 ALTER TABLE `status_visite` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `suiviappel`
--

DROP TABLE IF EXISTS `suiviappel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `suiviappel` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Num_Appel` bigint(20) DEFAULT NULL,
  `Type_Operation` varchar(100) DEFAULT NULL,
  `CodeMedecin` int(11) DEFAULT -1,
  `IdUtilisateur` varchar(6) DEFAULT NULL,
  `DateOp` datetime DEFAULT NULL,
  UNIQUE KEY `suiviappel_Id_IDX` (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=74 DEFAULT CHARSET=utf8 COMMENT='C''est l''historique de l''appel\r\nValeurs de Type_Operation : Création, Pré-attribution, Attribution, Reprise, Annulation,  Acquitement, Début de visite, Terminée  ';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `suiviappel`
--

LOCK TABLES `suiviappel` WRITE;
/*!40000 ALTER TABLE `suiviappel` DISABLE KEYS */;
INSERT INTO `suiviappel` VALUES (5,3000000,'Création',-1,'1401','2019-06-13 12:51:49'),(6,3000001,'Création',-1,'1401','2019-07-02 15:35:12'),(9,3000002,'Création',-1,'1401','2019-07-03 17:49:13'),(12,3000001,'Modifié',-1,'1401','2019-07-05 11:37:59'),(13,3000001,'Modifié',-1,'1401','2019-07-05 11:39:02'),(14,3000001,'Modifié',-1,'1401','2019-07-05 12:21:45'),(15,3000001,'Modifié',-1,'1401','2019-07-05 12:22:41'),(16,3000001,'Modifié',-1,'1401','2019-07-05 12:46:55'),(17,3000001,'Modifié',-1,'1401','2019-07-05 14:11:34'),(18,3000001,'Modifié',-1,'1401','2019-07-05 14:18:35'),(19,3000001,'Modifié',-1,'1401','2019-07-05 14:26:16'),(20,3000001,'Modifié',-1,'1401','2019-07-05 14:48:24'),(21,3000001,'Modifié',-1,'1401','2019-07-05 18:18:14'),(22,3000001,'Modifié',-1,'1401','2019-07-05 18:26:20'),(23,3000001,'Modifié',-1,'1401','2019-07-05 18:34:27'),(24,3000001,'Modifié',-1,'1401','2019-07-05 18:36:02'),(25,3000001,'Modifié',-1,'1401','2019-07-05 18:36:42'),(26,3000001,'Modifié',-1,'1401','2019-07-05 18:37:22'),(27,3000001,'Modifié',-1,'1401','2019-07-05 18:38:30'),(28,3000001,'Modifié',-1,'1401','2019-07-05 18:40:08'),(29,3000001,'Modifié',-1,'1401','2019-07-05 18:43:19'),(30,3000001,'Attribution',2887,'1401','2019-07-15 17:17:02'),(31,3000001,'Modifié',2887,'1401','2019-07-19 11:00:44'),(32,3000001,'Modifié',2887,'1401','2019-07-19 11:13:34'),(33,3000003,'Création',-1,'1401','2019-07-19 12:36:03'),(34,3000003,'Attribution',1401,'1401','2019-07-19 12:36:29'),(35,3000002,'Pré-attribution',1401,'1401','2019-07-19 12:37:08'),(36,3000002,'Reprise',1401,'1401','2019-07-19 12:39:05'),(37,3000003,'Reprise',1401,'1401','2019-07-19 13:48:58'),(38,3000003,'Attribution',1401,'1401','2019-07-19 17:12:34'),(39,3000002,'Pré-attribution',1401,'1401','2019-07-19 17:12:37'),(40,3000003,'Reprise',1401,'1401','2019-07-19 17:12:53'),(41,3000003,'Attribution',1401,'1401','2019-07-19 17:16:01'),(42,3000000,'Pré-attribution',1401,'1401','2019-07-19 17:16:03'),(43,3000000,'Reprise',1401,'1401','2019-07-19 17:16:16'),(44,3000003,'Reprise',1401,'1401','2019-07-19 17:16:18'),(45,3000002,'Reprise',1401,'1401','2019-07-19 17:16:20'),(46,3000003,'Attribution',1401,'1401','2019-07-19 17:16:59'),(47,3000002,'Pré-attribution',1401,'1401','2019-07-19 17:17:03'),(48,3000003,'Reprise',1401,'1401','2019-07-19 17:17:13'),(49,3000003,'Attribution',1401,'1401','2019-07-19 17:17:37'),(50,3000003,'Reprise',1401,'1401','2019-07-19 17:17:43'),(51,3000002,'Reprise',1401,'1401','2019-07-19 17:17:45'),(52,3000003,'Attribution',1401,'1401','2019-07-19 17:17:54'),(53,3000003,'Reprise',1401,'1401','2019-07-19 17:20:55'),(54,3000003,'Attribution',1401,'1401','2019-07-19 17:21:02'),(55,3000003,'Reprise',1401,'1401','2019-07-19 17:23:36'),(56,3000003,'Attribution',1401,'1401','2019-07-19 17:23:43'),(57,3000003,'Reprise',1401,'1401','2019-07-19 17:25:46'),(58,3000003,'Attribution',1401,'1401','2019-07-19 17:25:52'),(59,3000003,'Reprise',1401,'1401','2019-07-19 17:28:22'),(60,3000003,'Attribution',1401,'1401','2019-07-19 17:28:26'),(61,3000002,'Pré-attribution',1401,'1401','2019-07-19 17:28:46'),(62,3000002,'Reprise',1401,'1401','2019-07-19 17:29:02'),(63,3000003,'Reprise',1401,'1401','2019-07-19 17:29:18'),(64,3000003,'Attribution',1401,'1401','2019-07-19 17:29:24'),(65,3000002,'Pré-attribution',1401,'1401','2019-07-19 17:29:29'),(66,3000003,'Reprise',1401,'1401','2019-07-19 17:29:33'),(67,3000002,'Attribution',1401,'1401','2019-07-19 17:29:33'),(68,3000002,'Reprise',1401,'1401','2019-07-19 17:29:37'),(69,3000002,'Attribution',1401,'1401','2019-07-19 17:29:42'),(70,3000002,'Reprise',1401,'1401','2019-07-19 17:30:12'),(71,3000003,'Attribution',1401,'1401','2019-07-19 17:30:32'),(72,3000002,'Pré-attribution',1401,'1401','2019-07-19 17:30:38'),(73,3000003,'Reprise',1401,'1401','2019-07-19 17:30:52');
/*!40000 ALTER TABLE `suiviappel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `utilisateur`
--

DROP TABLE IF EXISTS `utilisateur`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `utilisateur` (
  `IdUtilisateur` varchar(6) NOT NULL,
  `Password` varchar(250) DEFAULT NULL,
  `Droit` int(11) DEFAULT NULL,
  `Nom` varchar(100) DEFAULT NULL,
  `Prenom` varchar(100) DEFAULT NULL,
  `IdSmartrapport` varchar(10) DEFAULT NULL,
  `IdCTI` varchar(10) DEFAULT NULL,
  `NumPoste` varchar(20) DEFAULT NULL,
  UNIQUE KEY `utilisateur_IdUtilisateur_IDX` (`IdUtilisateur`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `utilisateur`
--

LOCK TABLES `utilisateur` WRITE;
/*!40000 ALTER TABLE `utilisateur` DISABLE KEYS */;
INSERT INTO `utilisateur` VALUES ('1401','AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA3socpxZwJUWDU6sAadttTQAAAAACAAAAAAADZgAAwAAAABAAAADQRfd1wIyVw/DulEuON02nAAAAAASAAACgAAAAEAAAAM4o/YVllwAd9VF8JVj1ORwIAAAAAcJACL4pV8kUAAAAlSW+mFUN+pAnxPQvZEx7tocx5zM=',100,'MERCIER','Dominique','adm','2926','2926');
/*!40000 ALTER TABLE `utilisateur` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'regulation'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-07-23 16:26:28
