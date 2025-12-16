USE Salle_Sport;
-- ===============================================================================

INSERT INTO Coach (nom, prenom, Spécialité) VALUES 
('Schwarzy', 'Arnold', 'Musculation'),
('Stallone', 'Sylvester', 'Boxe'),
('Williams', 'Serena', 'Tennis'),
('Riner', 'Teddy', 'Judo'),
('Bolt', 'Usain', 'Athlétisme'),
('Comaneci', 'Nadia', 'Gymnastique'),
('Phelps', 'Michael', 'Natation'),
('Biles', 'Simone', 'Gymnastique'), -- Spécialité commune
('Ali', 'Muhammad', 'Boxe'),         -- Spécialité commune
('Lee', 'Bruce', 'Arts Martiaux'),
('Fonda', 'Jane', 'Fitness'),
('Norris', 'Chuck', 'Self Defense');

-- ==============================================================================

INSERT INTO Utilisateur (email, mdp, Nom, Prenom, rôle) VALUES 
-- Les 3 AP (Admins Principaux)
('admin1@gym.com', 'pass', 'Boss', 'Hugo', 'AP'),
('admin2@gym.com', 'pass', 'Chief', 'Sarah', 'AP'),
('admin3@gym.com', 'pass', 'Patron', 'Marc', 'AP'),
-- Les 3 AS (Admins Secondaires)
('staff1@gym.com', 'pass', 'Manager', 'Lea', 'AS'),
('staff2@gym.com', 'pass', 'Staff', 'Tom', 'AS'),
('staff3@gym.com', 'pass', 'Staff', 'Zoe', 'AS'),
-- L'Evaluateur (Ev)
('eval1@gym.com', 'pass', 'Evaluateur', 'Alex', 'Ev');

-- =============================================================================

INSERT INTO Utilisateur (email, mdp, Nom, Prenom, rôle) VALUES 
-- Groupe 1 (Futurs Actifs - 45 pers)
('mb1@mail.com', '1234', 'Dupont', 'Jean', 'MB'), ('mb2@mail.com', '1234', 'Durand', 'Paul', 'MB'),
('mb3@mail.com', '1234', 'Martin', 'Julie', 'MB'), ('mb4@mail.com', '1234', 'Bernard', 'Luc', 'MB'),
('mb5@mail.com', '1234', 'Thomas', 'Emma', 'MB'), ('mb6@mail.com', '1234', 'Petit', 'Lea', 'MB'),
('mb7@mail.com', '1234', 'Robert', 'Max', 'MB'), ('mb8@mail.com', '1234', 'Richard', 'Leo', 'MB'),
('mb9@mail.com', '1234', 'Simon', 'Chloé', 'MB'), ('mb10@mail.com', '1234', 'Michel', 'Tom', 'MB'),
('mb11@mail.com', '1234', 'Lefebvre', 'Hugo', 'MB'), ('mb12@mail.com', '1234', 'Leroy', 'Alice', 'MB'),
('mb13@mail.com', '1234', 'Moreau', 'Eva', 'MB'), ('mb14@mail.com', '1234', 'Laurent', 'Noe', 'MB'),
('mb15@mail.com', '1234', 'Lefevre', 'Lola', 'MB'), ('mb16@mail.com', '1234', 'Garcia', 'Enzo', 'MB'),
('mb17@mail.com', '1234', 'David', 'Jules', 'MB'), ('mb18@mail.com', '1234', 'Bertrand', 'Mila', 'MB'),
('mb19@mail.com', '1234', 'Roux', 'Gabriel', 'MB'), ('mb20@mail.com', '1234', 'Vincent', 'Louise', 'MB'),
('mb21@mail.com', '1234', 'Fournier', 'Adam', 'MB'), ('mb22@mail.com', '1234', 'Morel', 'Jade', 'MB'),
('mb23@mail.com', '1234', 'Girard', 'Louis', 'MB'), ('mb24@mail.com', '1234', 'Andre', 'Lina', 'MB'),
('mb25@mail.com', '1234', 'Lefevre', 'Ethan', 'MB'), ('mb26@mail.com', '1234', 'Mercier', 'Nina', 'MB'),
('mb27@mail.com', '1234', 'Dupuis', 'Mael', 'MB'), ('mb28@mail.com', '1234', 'Guuerin', 'Rose', 'MB'),
('mb29@mail.com', '1234', 'Boyer', 'Sacha', 'MB'), ('mb30@mail.com', '1234', 'Garnier', 'Anna', 'MB'),
('mb31@mail.com', '1234', 'Chevalier', 'Liam', 'MB'), ('mb32@mail.com', '1234', 'Francois', 'Zoe', 'MB'),
('mb33@mail.com', '1234', 'Legrand', 'Noah', 'MB'), ('mb34@mail.com', '1234', 'Gauthier', 'Mia', 'MB'),
('mb35@mail.com', '1234', 'Garcia', 'Timeo', 'MB'), ('mb36@mail.com', '1234', 'Perrin', 'Ines', 'MB'),
('mb37@mail.com', '1234', 'Robin', 'Malo', 'MB'), ('mb38@mail.com', '1234', 'Clement', 'Julia', 'MB'),
('mb39@mail.com', '1234', 'Morin', 'Lucas', 'MB'), ('mb40@mail.com', '1234', 'Nicolas', 'Manon', 'MB'),
('mb41@mail.com', '1234', 'Henry', 'Arthur', 'MB'), ('mb42@mail.com', '1234', 'Roussel', 'Lena', 'MB'),
('mb43@mail.com', '1234', 'Mathieu', 'Theo', 'MB'), ('mb44@mail.com', '1234', 'Gautier', 'Sarah', 'MB'),
('mb45@mail.com', '1234', 'Masson', 'Leon', 'MB'),

-- Groupe 2 (Futurs En Attente - 7 pers)
('wait1@mail.com', '1234', 'Attente', 'Jean', 'MB'), ('wait2@mail.com', '1234', 'Attente', 'Henri', 'MB'),
('wait3@mail.com', '1234', 'Attente', 'Antoine', 'MB'), ('wait4@mail.com', '1234', 'Attente', 'Nicolas', 'MB'),
('wait5@mail.com', '1234', 'Attente', 'Marc', 'MB'), ('wait6@mail.com', '1234', 'Attente', 'Adriena', 'MB'),
('wait7@mail.com', '1234', 'Attente', 'Julie', 'MB'),

-- Groupe 3 (Futurs Refusés - 5 pers)
('ref1@mail.com', '1234', 'Refuse', 'Marie', 'MB'), ('ref2@mail.com', '1234', 'Refuse', 'Romain', 'MB'),
('ref3@mail.com', '1234', 'Refuse', 'Mayen', 'MB'), ('ref4@mail.com', '1234', 'Refuse', 'Laura', 'MB'),
('ref5@mail.com', '1234', 'Refuse', 'Maxime', 'MB'),

-- Groupe 4 (Futurs Bannis - 5 pers)
('ban1@mail.com', '1234', 'Banni', 'Pierrick', 'MB'), ('ban2@mail.com', '1234', 'Banni', 'Justine', 'MB'),
('ban3@mail.com', '1234', 'Banni', 'Noam', 'MB'), ('ban4@mail.com', '1234', 'Banni', 'Anaïs', 'MB'),
('ban5@mail.com', '1234', 'Banni', 'Romane', 'MB'),

-- Groupe 5 (Futurs Quittés - 12 pers)
('quit1@mail.com', '1234', 'Parti', 'Niels', 'MB'), ('quit2@mail.com', '1234', 'Parti', 'Quentin', 'MB'),
('quit3@mail.com', '1234', 'Parti', 'Aurélien', 'MB'), ('quit4@mail.com', '1234', 'Parti', 'Tom', 'MB'),
('quit5@mail.com', '1234', 'Parti', 'Aurélie', 'MB'), ('quit6@mail.com', '1234', 'Parti', 'Mael', 'MB'),
('quit7@mail.com', '1234', 'Parti', 'Nicolle', 'MB'), ('quit8@mail.com', '1234', 'Parti', 'Fabrice', 'MB'),
('quit9@mail.com', '1234', 'Parti', 'Ludivine', 'MB'), ('quit10@mail.com', '1234', 'Parti', 'Fabien', 'MB'),
('quit11@mail.com', '1234', 'Parti', 'Guillaume', 'MB'), ('quit12@mail.com', '1234', 'Parti', 'Fabien', 'MB');

-- =======================================================================================

-- Actuellement, tout le monde est 'EN_ATTENTE' (grâce au Trigger).
-- On va les modifier par lots en utilisant les IDs.
-- Note : Les IDs commencent à 7 (car 6 staff avant).

-- 1. Les 45 Actifs (IDs 7 à 51)
-- Ils sont validés par l'Admin #1 (Hugo)
UPDATE Dossier_Mb 
SET statut = 'ACTIF', valide_par = 1, date_valid_admin = NOW()
WHERE id_user BETWEEN 7 AND 51;

-- 2. Les 7 En Attente (IDs 52 à 58)
-- On ne fait rien, ils sont déjà 'EN_ATTENTE' par défaut.

-- 3. Les 5 Refusés (IDs 59 à 63)
UPDATE Dossier_Mb
SET statut = 'REFUSE', valide_par = 1, date_valid_admin = NOW()
WHERE id_user BETWEEN 59 AND 63;

-- 4. Les 5 Bannis (IDs 64 à 68)
UPDATE Dossier_Mb
SET statut = 'BAN', motif_ban = 'Non respect du règlement', valide_par = 1
WHERE id_user BETWEEN 64 AND 68;

-- 5. Les 12 Quittés (IDs 69 à 80)
UPDATE Dossier_Mb
SET statut = 'QUIT', valide_par = 1
WHERE id_user BETWEEN 69 AND 80;

-- ==============================================================================

INSERT INTO Activite (Nom_Ade, descri) VALUES 
('Musculation', 'Accès libre au plateau et conseils personnalisés'),
('Crossfit', 'Entraînement fonctionnel à haute intensité'),
('Yoga', 'Relaxation et souplesse'),
('Boxe', 'Combat et cardio'),
('Zumba', 'Danse fitness rythmé'),
('Pilates', 'Renforcement des muscles profonds'),
('Body Pump', 'Renforcement musculaire avec poids légers');

-- =============================================================================

-- A. SEANCES PASSÉES (Toujours calculées par rapport à "Aujourd'hui")
-- C'est indispensable pour que le système détecte les absences DÈS MAINTENANT.
INSERT INTO Seance (DateH_debut, durée, cap_max, id_coach, id_ade) VALUES 
(NOW() - INTERVAL 1 MONTH, 60, 20, 1, 1),  -- ID 1 (Passée)
(NOW() - INTERVAL 2 DAY, 45, 15, 2, 4),    -- ID 2 (Passée)
(NOW() - INTERVAL 1 DAY, 60, 25, 3, 3),    -- ID 3 (Passée)
(NOW() - INTERVAL 5 HOUR, 90, 10, 8, 2);   -- ID 4 (Passée)

-- B. SEANCES FUTURES (Fixées à JANVIER/FÉVRIER 2026)
-- Elles apparaîtront forcément dans le futur pour l'évaluation.
INSERT INTO Seance (DateH_debut, durée, cap_max, id_coach, id_ade) VALUES 
('2026-01-15 10:00:00', 60, 20, 1, 1),    -- ID 5 : Muscu (15 Janv 2026)
('2026-01-20 18:30:00', 60, 20, 4, 6),    -- ID 6 : Pilates (20 Janv 2026)
('2026-02-05 14:00:00', 45, 15, 5, 5),    -- ID 7 : Zumba (05 Fév 2026)
('2026-03-01 09:00:00', 120, 30, 2, 4);   -- ID 8 : Stage Boxe (01 Mars 2026)

-- =============================================================================

-- A. HISTORIQUE (Sur les séances 1, 2, 3 - Passées)
-- Les dates d'inscription doivent être logiques (avant le cours).
INSERT INTO Inscri_Seance (id_user, id_seance, date_insc, present) VALUES 
(7, 1, NOW() - INTERVAL 35 DAY, TRUE),
(8, 1, NOW() - INTERVAL 35 DAY, TRUE),
(7, 2, NOW() - INTERVAL 3 DAY, TRUE),
(9, 3, NOW() - INTERVAL 2 DAY, TRUE);

-- B. LES ABSENTEISTES (Le Membre ID 45 va se faire bannir)
-- On l'inscrit aux séances passées (1, 2, 3) et on met PRESENT = FALSE
INSERT INTO Inscri_Seance (id_user, id_seance, date_insc, present) VALUES 
(45, 1, NOW() - INTERVAL 35 DAY, FALSE),
(45, 2, NOW() - INTERVAL 4 DAY, FALSE),
(45, 3, NOW() - INTERVAL 2 DAY, FALSE);
-- -> Résultat : 3 absences détectées immédiatement par la Vue.

-- C. LES INSCRIPTIONS FUTURES (Sur les séances 2026 : IDs 5, 6, 8)
-- On inscrit les gens "Aujourd'hui" (NOW) pour des cours en 2026.
INSERT INTO Inscri_Seance (id_user, id_seance, date_insc, present) VALUES 
(10, 5, NOW(), FALSE),
(11, 5, NOW(), FALSE),
(12, 6, NOW(), FALSE),
(7, 8, NOW(), FALSE);