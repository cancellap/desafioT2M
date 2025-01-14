import React from "react";
import { useNavigate } from "react-router-dom";
import styles from "./BotaoFlutuante.module.css";
import { FaPlus } from 'react-icons/fa';

export default function BotaoFlutuante() {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate("/createProjeto");
  };

  return (
    <button className={styles.floatingButton} onClick={handleClick}>
     <FaPlus size={30} color="white" />
    </button>
  );
}
