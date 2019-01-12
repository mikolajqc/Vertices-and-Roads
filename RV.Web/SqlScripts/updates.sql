ALTER TABLE roads
ADD COLUMN isView BOOLEAN DEFAULT FALSE;

CREATE VIEW RoadView
AS SELECT
id, source as sourceid, target as targetid, isview
FROM roads;