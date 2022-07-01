 
create table `casos_covid`(
`city` VARCHAR(255) NOT NULL,
`city_ibge_code` VARCHAR(255) NOT NULL,
`date` VARCHAR(255) NOT NULL,
`epidemiological_week` INT NOT NULL,
`estimated_population` INT NOT NULL, 
`estimated_population_2019` INT NOT NULL,
`is_last`VARCHAR(255) NOT NULL,
`last_available_confirmed` INT NOT NULL,
`last_available_confirmed_per_100k_inhabitants` INT NOT NULL,
`last_available_date` VARCHAR(255) NOT NULL,
`last_available_death_rate` INT NOT NULL, 
`order_for_place` INT NOT NULL, 
`place_type` VARCHAR(255) NOT NULL,
`state` VARCHAR(255) NOT NULL,
`new_confirmed` VARCHAR(255) NOT NULL, 
`new_deaths` VARCHAR(255) NOT NULL,
PRIMARY KEY (`city_ibge_code`)
) ENGINE=InnoDB;

 