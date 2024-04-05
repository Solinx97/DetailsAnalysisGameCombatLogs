-- Combat
insert into dbo.Combat
values ('Dungeon #1', 'Boss #1', 3000, 2500, 4000, 500, 0, 0, 'TRUE', '09.26.2022 11:10:00', '09.26.2022 11:12:00', 1)

-- CombatLog
insert into dbo.CombatLog
values ('Combat #1', '09.27.2022 10:15:00', 'TRUE')

-- CombatLogByUser
insert into dbo.Combat
values ('Dungeon #1', 'Boss #1', 3000, 2500, 4000, 500, 0, 0, 'TRUE', '09.26.2022 11:10:00', '09.26.2022 11:12:00', 1)

-- CombatPlayer
insert into dbo.CombatPlayer
values ('Player #1', 650, 2750, 4255, 3210, 0, 1)

-- DamageDone
insert into dbo.DamageDone
values (275, '00:35', 'Player #1', 'Enemy #1', 'Spell #1', 'FALSE', 'FALSE', 'FALSE', 'FALSE', 'FALSE', 'FALSE', 'FALSE', 1)

-- DamageDoneGeneral
insert into dbo.DamageDoneGeneral
values (2750, 943, 'Spell #1', 2, 0, 18, 275, 650, 462, 1)

-- DamageTaken
insert into dbo.DamageTaken
values (148, '00:29', 'Enemy #1', 'Player #1', 'Spell #1', 'FALSE', 0, 0, 24, 192, 20, 'FALSE', 'FALSE', 'FALSE', 'FALSE', 'FALSE', 'TRUE', 1)

-- DamageTakenGeneral
insert into dbo.DamageTakenGeneral
values (3210, 745, 'Spell #1', 0, 0, 11, 411, 725, 568, 1)

-- HealDone
insert into dbo.HealDone
values (433, '00:30', 20, 513, 'Player #1', 'Player #2','Spell #1', 'FALSE', 'FALSE', 1)

-- HealDoneGeneral
insert into dbo.HealDoneGeneral
values (4255, 865, 'Spell #1', 0, 23, 411, 725, 568, 1)

-- ResourceRecovery
insert into dbo.ResourceRecovery
values (64, '00:26', 'Player #1', 1)

-- ResourceRecoveryGeneral
insert into dbo.ResourceRecoveryGeneral
values (650, 211, 'Spell #1', 9, 49, 227, 138, 1)