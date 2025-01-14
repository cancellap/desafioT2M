import React from "react";
import { NavLink } from "react-router-dom";
import styles from "./NavBar.module.css";

export default function  NavBar  ()  {
  return (
    <nav className={styles.navbar}>
      <div className={styles["navbar-logo"]}>Gerenciador de Projetos</div>
      <div className={styles["navbar-links"]}>
        <NavLink
          to="/projetosGerais"
          className={({ isActive }) =>
            `${styles["navbar-link"]} ${isActive ? styles.active : ""}`
          }
        >
          Projetos Gerais
        </NavLink>
        <NavLink
          to="/projetosPropios"
          className={({ isActive }) =>
            `${styles["navbar-link"]} ${isActive ? styles.active : ""}`
          }
        >
          Projetos Próprios
        </NavLink>
      </div>
    </nav>
  );
};

