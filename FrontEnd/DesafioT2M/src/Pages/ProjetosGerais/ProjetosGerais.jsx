import React, { useState, useEffect } from "react";
import ProjetoCard from "../../Components/ProjetoCard/ProjetoCard";
import styles from "./ProjetosGerais.module.css";
import BotaoFlutuante from "../../Components/BotaoAddProjeto/BotaoFlutuante";
import { getProjetos } from "../../Service/Api";

export default function ProjetosGerais() {
  const [projetos, setProjetos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchProjetos = async () => {
      try {
        const data = await getProjetos();
        setProjetos(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchProjetos();
  }, []);

  if (loading) return <p>Carregando...</p>;
  if (error) return <p>Erro: {error}</p>;

  return (
    <div className={styles.main}>
      <BotaoFlutuante />
      <h1>Projetos Gerais</h1>
      {projetos.length > 0 ? (
        <div className={styles.projetosGerais}>
          {projetos.map((projeto) => (
            <ProjetoCard
              key={projeto.id}
              id={projeto.id}
              nome={projeto.nome}
              dataInicio={projeto.dataInicio}
              dataTermino={projeto.dataTermino}
            />
          ))}
        </div>
      ) : (
        <p>Nenhum projeto encontrado.</p>
      )}
    </div>
  );
}
