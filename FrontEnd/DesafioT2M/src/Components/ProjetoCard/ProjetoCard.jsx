import React from "react";
import { useNavigate } from "react-router-dom";
import styles from "./ProjetoCard.module.css";

export default function ProjetoCard({ id, nome, dataInicio, dataTermino }) {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(`/projeto/${id}`);
  };

  return (
    <div className={styles.card} onClick={handleClick}>
      <h2>{nome}</h2>
      <p>Início: {new Date(dataInicio).toLocaleDateString()}</p>
      <p>Término: {new Date(dataTermino).toLocaleDateString()}</p>
    </div>
  );
}
