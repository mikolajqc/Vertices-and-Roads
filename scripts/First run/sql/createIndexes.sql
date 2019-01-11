ALTER TABLE roads ADD COLUMN source integer;
ALTER TABLE roads ADD COLUMN target integer;
CREATE INDEX roads_source_idx ON roads (source);
CREATE INDEX roads_target_idx ON roads (target);
