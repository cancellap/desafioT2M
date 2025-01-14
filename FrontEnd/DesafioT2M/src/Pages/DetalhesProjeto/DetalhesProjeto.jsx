import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import styles from "./DetalhesProjeto.module.css";
import { getUsuario } from "../../Service/Api";
import { FaEdit } from "react-icons/fa";
import { FaPlus } from "react-icons/fa";
import ModalEditarTarefa from "../../Components/ModalEditarTarefa/ModalEditarTarefa";

export default function DetalhesProjeto() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [projeto, setProjeto] = useState(null);
  const [tarefasComUsuarios, setTarefasComUsuarios] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedTarefa, setSelectedTarefa] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const getProjeto = async () => {
    try {
      const response = await fetch(`http://localhost:5029/api/projeto/${id}`);
      if (!response.ok) {
        throw new Error("Erro ao buscar detalhes do projeto");
      }
      const data = await response.json();
      setProjeto(data);

      const tarefasComUsuarios = await Promise.all(
        data.tarefas.map(async (tarefa) => {
          const usuarioData = await getUsuario(tarefa.usuarioId);
          return {
            ...tarefa,
            usuarioNome: usuarioData.nome,
          };
        })
      );
      setTarefasComUsuarios(tarefasComUsuarios);
    } catch (err) {
      setError(err.message);
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        await getProjeto();
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  const getStatus = (statusTarefa) => {
    switch (statusTarefa) {
      case "NaoIniciado":
        return "Não Iniciada";
      case "EmAndamento":
        return "Em Andamento";
      default:
        return "Desconhecido";
    }
  };

  const handleEditClick = (tarefa) => {
    setSelectedTarefa(tarefa);
    setIsModalOpen(true);
  };

  const handleSaveTarefa = async (updatedTarefa) => {
    setTarefasComUsuarios((prevTarefas) =>
      prevTarefas.map((tarefa) =>
        tarefa.id === updatedTarefa.id ? updatedTarefa : tarefa
      )
    );

    await getProjeto();
  };

  if (loading) return <p>Carregando...</p>;
  if (error) return <p>Erro: {error}</p>;

  return (
    <div className={styles.card}>
      <h1>{projeto.nome}</h1>
      <p>Início: {new Date(projeto.dataInicio).toLocaleDateString()}</p>
      <p>Término: {new Date(projeto.dataTermino).toLocaleDateString()}</p>
      <p className={styles.descricao}>{projeto.descricao}</p>

      <div className={styles.tarefasContainer}>
        <h2 className={styles.tarefasTitle}>
          Tarefas{" "}
          <FaPlus
            size={25}
            style={{
              cursor: "pointer",
              marginTop: "1",
            }}
          />
        </h2>
        {tarefasComUsuarios && tarefasComUsuarios.length > 0 ? (
          <ul className={styles.tarefasLista}>
            {tarefasComUsuarios.map((tarefa) => (
              <li
                key={tarefa.id}
                className={`${styles.tarefaItem} ${
                  tarefa.statusTarefa === "Concluida" ? styles.completed : ""
                }`}
              >
                <span className={styles.tarefaNome}>{tarefa.nome}</span>
                <span>{tarefa.usuarioNome}</span>
                <span
                  className={`${styles.tarefaStatus} ${
                    tarefa.statusTarefa === "NaoIniciado"
                      ? styles.naoIniciado
                      : tarefa.statusTarefa === "EmAndamento"
                      ? styles.emAndamento
                      : ""
                  }`}
                >
                  {getStatus(tarefa.statusTarefa)}{" "}
                </span>
                <span>
                  <FaEdit
                    color="black"
                    style={{
                      fontSize: "20px",
                      cursor: "pointer",
                      marginTop: "1",
                    }}
                    onClick={() => handleEditClick(tarefa)}
                  />
                </span>
              </li>
            ))}
          </ul>
        ) : (
          <p>Nenhuma tarefa encontrada.</p>
        )}
      </div>

      <button
        className={styles.button}
        onClick={() => navigate("/projetosGerais")}
      >
        Voltar
      </button>

      {isModalOpen && (
        <ModalEditarTarefa
          tarefa={selectedTarefa}
          onClose={() => setIsModalOpen(false)}
          onSave={handleSaveTarefa}
        />
      )}
    </div>
  );
}
