import React from "react";
import { NavLink, useNavigate } from "react-router-dom";
import styles from "./NavBar.module.css";

export default function NavBar() {
  const navigate = useNavigate();

  const logout = () => {
    localStorage.clear();
    navigate("/login");
  };

  return (
    <nav className={styles.navbar}>
      <div className={styles.navbarLogo}>Gerenciador de Projetos</div>
      <div className={styles.navbarLinks}>
        <NavLink
          to="/projetosGerais"
          className={({ isActive }) =>
            `${styles.navbarLink} ${isActive ? styles.active : ""}`
          }
        >
          Projetos Gerais
        </NavLink>
        <NavLink
          to="/projetosPropios"
          className={({ isActive }) =>
            `${styles.navbarLink} ${isActive ? styles.active : ""}`
          }
        >
          Projetos Próprios
        </NavLink>
        <button className={styles.logoutButton} onClick={logout}>
          Sair
        </button>
      </div>
    </nav>
  );
}
