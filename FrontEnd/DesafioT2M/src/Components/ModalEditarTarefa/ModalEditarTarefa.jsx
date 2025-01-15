import React, { useState, useEffect } from "react";
import { Formik, Field, Form, ErrorMessage } from "formik";
import * as Yup from "yup";
import { updateTarefa } from "../../Service/Api";
import styles from "./ModalEditarTarefa.module.css";

const validationSchema = Yup.object({
  nome: Yup.string().required("Nome da tarefa é obrigatório"),
  descricao: Yup.string().required("Descrição é obrigatória"),
  prazo: Yup.string().required("Prazo é obrigatório"),
  statusTarefa: Yup.string()
  .oneOf(["NaoIniciado", "EmAndamento", "Concluida"], "Status inválido")
  .required("Status da tarefa é obrigatório"),
});

export default function ModalEditarTarefa({ tarefa, onClose, onSave }) {
  const [initialValues, setInitialValues] = useState({
    nome: "",
    descricao: "",
    prazo: "",
    statusTarefa: "NaoIniciado",
  });

  const statusMapReverse = {
    0: "NaoIniciado",
    1: "EmAndamento",
  };

  useEffect(() => {
    if (tarefa) {
      setInitialValues({
        nome: tarefa.nome || "",
        descricao: tarefa.descricao || "",
        prazo: tarefa.prazo || "",
        statusTarefa: statusMapReverse[tarefa.statusTarefa] || "NaoIniciado",
      });
    }
  }, [tarefa]);

  const handleSave = async (values) => {
    if (!tarefa.id) {
      console.error("ID da tarefa é necessário");
      return;
    }

    const updatedTarefa = {
      nome: values.nome,
      descricao: values.descricao,
      prazo: values.prazo,
      statusTarefa: values.statusTarefa,
      usuarioId: tarefa.usuarioId,
      projetoId: tarefa.projetoId,
    };
    const token = localStorage.getItem("token");
    try {
      const resposta = await updateTarefa(tarefa.id, updatedTarefa, token);
      console.log("Tarefa atualizada com sucesso:", resposta);
      onSave(updatedTarefa);
      onClose();
    } catch (error) {
      if (error.response) {
        if (error.response.status === 403) {
          alert("Você não tem permissão para editar esta tarefa.");
        } else {
          alert(`Erro ao atualizar a tarefa. Status: ${error.response.status}`);
        }
      } else {
        alert("Erro ao tentar atualizar a tarefa.");
      }
    }
  };

  return (
    <div className={styles.modalOverlay}>
      <div className={styles.modalContent}>
        <h2>Editar Tarefa</h2>
        <Formik
          initialValues={initialValues}
          validationSchema={validationSchema}
          onSubmit={(values) => {
            const statusMap = {
              NaoIniciado: 0,
              EmAndamento: 1,
              Concluida: 2,
            };
            handleSave({
              ...values,
              statusTarefa: statusMap[values.statusTarefa],
            });
          }}
        >
          <Form>
            <div className={styles.label}>
              <label>Nome da Tarefa:</label>
              <Field className={styles.input} type="text" name="nome" />
              <ErrorMessage
                name="nome"
                component="div"
                className={styles.error}
              />
            </div>
            <div className={styles.label}>
              <label>Descrição:</label>
              <Field className={styles.input} type="text" name="descricao" />
              <ErrorMessage
                name="descricao"
                component="div"
                className={styles.error}
              />
            </div>
            <div className={styles.label}>
              <label>Prazo:</label>
              <Field className={styles.input} type="date" name="prazo" />
              <ErrorMessage
                name="prazo"
                component="div"
                className={styles.error}
              />
            </div>
            <div className={styles.label}>
              <label>Status da Tarefa:</label>
              <Field as="select" className={styles.select} name="statusTarefa">
                <option value="NaoIniciado">Não Iniciada</option>
                <option value="EmAndamento">Em Andamento</option>
                <option value="Concluida">Concluída</option>
              </Field>
              <ErrorMessage
                name="statusTarefa"
                component="div"
                className={styles.error}
              />
            </div>
            <div className={styles.buttons}>
              <button
                className={styles.cancelButton}
                type="button"
                onClick={onClose}
              >
                Cancelar
              </button>
              <button className={styles.button} type="submit">
                Salvar
              </button>
            </div>
          </Form>
        </Formik>
      </div>
    </div>
  );
}
