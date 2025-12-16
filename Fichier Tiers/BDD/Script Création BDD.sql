DROP DATABASE IF EXISTS Salle_Sport;
CREATE DATABASE IF NOT EXISTS Salle_Sport;
USE Salle_Sport;

CREATE TABLE Utilisateur (
	id_User int AUTO_INCREMENT,
    email VARCHAR(100) UNIQUE NOT NULL,
    mdp VARCHAR(100) NOT NULL,
    Nom VARCHAR(50) NOT NULL,
    Prenom VARCHAR(50) NOT NULL,
    rôle ENUM('AP', 'AS', 'Ev', 'Mb') DEFAULT 'MB',
    PRIMARY KEY (id_User)
    );

CREATE TABLE Dossier_Mb (
	id_user INT,
    statut ENUM('EN_ATTENTE', 'ACTIF', 'REFUSE', 'BAN', 'QUIT') DEFAULT 'EN_ATTENTE',
    motif_ban VARCHAR(255) NULL,
    date_creat_dossier DATETIME DEFAULT CURRENT_TIMESTAMP,
    date_valid_admin DATETIME NULL,
    valide_par INT NULL,
    
    PRIMARY KEY (id_User),
    CONSTRAINT fk_dossier_User FOREIGN KEY (id_user) REFERENCES Utilisateur(id_User) ON DELETE CASCADE,
    CONSTRAINT fk_dossier_validpar FOREIGN KEY (valide_par) REFERENCES Utilisateur(id_User)
    );
    
CREATE TABLE Coach (
		id_Coach INT AUTO_INCREMENT,
        Nom VARCHAR(50) NOT NULL,
        Prenom VARCHAR(50) NOT NULL,
        Spécialité VARCHAR(150),
        
        PRIMARY KEY (id_Coach)
        );
        
CREATE TABLE Activite (
	id_Ade INT AUTO_INCREMENT,
    Nom_Ade VARCHAR(50) UNIQUE NOT NULL,
    descri TEXT,
    
    PRIMARY KEY (id_Ade)
    );

CREATE TABLE Seance (
	id_Seance INT AUTO_INCREMENT,
    dateH_debut DATETIME NOT NULL,
    durée INT DEFAULT 60,
    cap_max INT DEFAULT 20,
    id_coach INT NOT NULL,
    id_ade INT NOT NULL,
    
    PRIMARY KEY (id_Seance),
    CONSTRAINT fk_sCoach FOREIGN KEY (id_coach) REFERENCES Coach(id_Coach),
    CONSTRAINT fk_sAde FOREIGN KEY (id_ade) REFERENCES Activite(id_Ade)
    );

CREATE TABLE Inscri_Seance (
	id_user INT NOT NULL,
    id_seance INT NOT NULL,
    date_insc DATETIME DEFAULT CURRENT_TIMESTAMP,
    present BOOL DEFAULT FALSE,
    
    PRIMARY KEY(id_user, id_seance),
    CONSTRAINT fk_inscUser FOREIGN KEY (id_user) REFERENCES Utilisateur(id_User) ON DELETE CASCADE,
    CONSTRAINT fk_inscSeance FOREIGN KEY (id_seance) REFERENCES Seance(id_Seance) ON DELETE CASCADE
    );

-- =====================================================================

CREATE VIEW Vue_Mb_Absent AS
SELECT U.id_User, U.nom, U.prenom, U.email, COUNT(I.id_Seance) AS total_absences
FROM UTILISATEUR U
JOIN Inscri_Seance I ON U.id_User = I.id_user
JOIN Seance S ON I.id_seance = S.id_Seance
WHERE S.dateH_debut < NOW()
	AND I.present = FALSE
    AND U.rôle = 'MB'
GROUP BY U.id_User
HAVING total_absences >= 3;

-- ======================================================================

DELIMITER //

CREATE TRIGGER Before_Ins_User
BEFORE INSERT ON Utilisateur FOR EACH ROW
BEGIN
	IF NEW.rôle is NULL THEN SET NEW.rôle = 'MB';
    END IF;
END //

CREATE TRIGGER After_Ins_User_DossierMb
AFTER INSERT ON Utilisateur FOR EACH ROW
BEGIN
	IF NEW.rôle = 'MB' THEN
		INSERT INTO Dossier_Mb (id_user, statut, date_creat_dossier)
        VALUES (NEW.id_user, 'EN_ATTENTE', NOW());
	END IF;
END //

CREATE TRIGGER Before_Del_User
BEFORE DELETE ON Utilisateur FOR EACH ROW
BEGIN
	IF OLD.rôle != 'MB' THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'IMPOSSIBLE DE SUPPRIMER UN STAFF';
	END IF;
END //

CREATE TRIGGER Before_Ins_Inscri_CheckStat
BEFORE INSERT ON Inscri_Seance FOR EACH ROW
BEGIN
	DECLARE v_statut VARCHAR(20);
	SELECT statut INTO v_statut FROM Dossier_MB WHERE id_user = NEW.id_user;
    
    IF v_statut != 'ACTIF' THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'DOSSIER DOIT ETRE ACTIF POUR REJOINDRE UNE SEANCE';
	END IF;
END //

DELIMITER ;

-- ============================================================================

DELIMITER //

CREATE PROCEDURE JeQuitte(IN p_id_user INT)
BEGIN
	UPDATE Dossier_Mb SET statut = 'QUIT' WHERE id_user = p_id_user;
    
    DELETE I
    FROM Inscri_Seance I
    INNER JOIN Seance S ON I.id_seance = S.id_Seance
    WHERE I.id_user = p_id_user
		AND S.dateH_debut > NOW();
END //

CREATE PROCEDURE Ban_Mb(IN p_id_user INT, IN p_raison VARCHAR(255))
BEGIN
	UPDATE Dossier_Mb
    SET statut = "BAN", motif_ban = p_raison
    WHERE id_user = p_id_user;
    
    DELETE I
    FROM Inscri_Seance I
    INNER JOIN Seance S ON I.id_seance = S.id_Seance
    WHERE I.id_user = p_id_user
		AND S.dateH_debut > NOW();
END //

DELIMITER ;

-- =============================================================================

DROP ROLE IF EXISTS 'R_AP', 'R_AS', 'R_Ev', 'R_Mb';

CREATE ROLE 'R_AP';
GRANT SELECT ON Utilisateur TO 'R_AP';
GRANT SELECT ON Dossier_Mb TO 'R_AP';
GRANT SELECT ON Vue_Mb_Absent TO 'R_AP';
GRANT SELECT ON Coach TO 'R_AP';
GRANT SELECT ON Activite TO 'R_AP';
GRANT SELECT ON Seance TO 'R_AP';
GRANT SELECT ON Inscri_Seance TO 'R_AP';
GRANT UPDATE (statut, date_valid_admin, valide_par) ON Dossier_Mb TO 'R_AP';
GRANT EXECUTE ON PROCEDURE Ban_Mb TO 'R_AP';

CREATE ROLE 'R_AS';
GRANT SELECT ON Utilisateur TO 'R_AS';	
GRANT SELECT ON Dossier_Mb TO 'R_AS';
GRANT SELECT, INSERT, UPDATE, DELETE ON Seance TO 'R_AS';
GRANT SELECT, INSERT, UPDATE, DELETE ON Activite TO 'R_AS';
GRANT SELECT, INSERT, UPDATE, DELETE ON Coach TO 'R_AS';
GRANT SELECT, DELETE ON Inscri_Seance TO 'R_AS';
GRANT UPDATE (present) ON Inscri_Seance TO 'R_AS';

CREATE ROLE 'R_Mb';
GRANT INSERT (Nom, Prenom, email, mdp) ON Utilisateur TO 'R_Mb';
GRANT SELECT ON Utilisateur TO 'R_Mb';
GRANT UPDATE (Nom, Prenom, email, mdp) ON Utilisateur TO 'R_Mb';
GRANT SELECT ON Dossier_Mb TO 'R_Mb';
GRANT EXECUTE ON PROCEDURE JeQuitte TO 'R_Mb';
GRANT SELECT ON Seance TO 'R_Mb';
GRANT SELECT ON Activite TO 'R_Mb';
GRANT SELECT (id_Coach, Nom, Prenom, Spécialité) ON Coach TO 'R_Mb';
GRANT SELECT, DELETE ON Inscri_Seance TO 'R_Mb';
GRANT INSERT (id_user, id_seance) ON Inscri_Seance TO 'R_Mb';

CREATE ROLE 'R_Ev';
GRANT SELECT (id_User, Nom, Prenom, rôle) ON Utilisateur TO 'R_Ev';
GRANT SELECT (id_Coach, Nom, Prenom, Spécialité) ON Coach TO 'R_Ev';
GRANT SELECT (id_user, statut, date_creat_dossier, motif_ban) ON Dossier_Mb TO 'R_Ev';
GRANT SELECT ON Activite TO 'R_Ev';
GRANT SELECT ON Seance TO 'R_Ev';
GRANT SELECT ON Inscri_Seance TO 'R_Ev';

FLUSH PRIVILEGES;
-- ==============================================================================

DROP USER IF EXISTS 'app_login'@'%';
CREATE USER 'app_login'@'%' IDENTIFIED BY 'login_pass';
GRANT SELECT ON Salle_Sport.Utilisateur TO 'app_login'@'%';

DROP USER IF EXISTS 'app_ap'@'%';
CREATE USER 'app_ap'@'%' IDENTIFIED BY 'ap_pass';
GRANT 'R_AP' TO 'app_ap'@'%';
SET DEFAULT ROLE 'R_AP' TO 'app_ap'@'%';

DROP USER IF EXISTS 'app_as'@'%';
CREATE USER 'app_as'@'%' IDENTIFIED BY 'as_pass';
GRANT 'R_AS' TO 'app_as'@'%';
SET DEFAULT ROLE 'R_AS' TO 'app_as'@'%';

DROP USER IF EXISTS 'app_mb'@'%';
CREATE USER 'app_mb'@'%' IDENTIFIED BY 'mb_pass';
GRANT 'R_Mb' TO 'app_mb'@'%';
SET DEFAULT ROLE 'R_Mb' TO 'app_mb'@'%';

DROP USER IF EXISTS 'app_ev'@'%';
CREATE USER 'app_ev'@'%' IDENTIFIED BY 'ev_pass';
GRANT 'R_Ev' TO 'app_ev'@'%';
SET DEFAULT ROLE 'R_Ev' TO 'app_ev'@'%';

FLUSH PRIVILEGES;