--

CREATE TABLE roads (
    id integer NOT NULL,
    geom geometry(LineString,4326)
);

--
-- Name: roads roads_id_idx; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY roads
    ADD CONSTRAINT roads_id_idx PRIMARY KEY (id);


--
-- Name: roads_geom_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX roads_geom_idx ON roads USING gist (geom);

