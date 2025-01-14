import React, { useState, useEffect } from "react";
import { Formik, Field, Form, ErrorMessage } from "formik";
import * as Yup from "yup";
import styles from "./ModalEditarTarefa.module.css";

const validationSchema = Yup.object({
  nome: Yup.string().required("Nome da tarefa é obrigatório"),
  descricao: Yup.string().required("Descrição é obrigatória"),
  prazo: Yup.string().required("Prazo é obrigatório"),
  statusTarefa: Yup.number().required("Status da tarefa é obrigatório"),
});

export default function ModalEditarTarefa({
  tarefa,
  onClose,
  onSave,
}) {
  const [initialValues, setInitialValues] = useState({
    nome: "",
    descricao: "",
    prazo: "",
    statusTarefa: 0,
  });

  useEffect(() => {
    if (tarefa) {
      setInitialValues({
        nome: tarefa.nome || "",
        descricao: tarefa.descricao || "",
        prazo: tarefa.prazo || "",
        statusTarefa: tarefa.statusTarefa || 0,
      });
    }
  }, [tarefa]);

  const getStatusTarefaValue = (status) => {
    switch (status) {
      case "NaoIniciado":
        return 0; 
      case "EmAndamento":
        return 1; 
      case "Concluida":
        return 2; 
      default:
        return 0; 
    }
  };

  const handleSave = async (values) => {
    if (!tarefa.id) {
      console.error("ID da tarefa é necessário");
      return;
    }

    const updatedTarefa = {
      nome: values.nome,
      descricao: values.descricao,
      prazo: values.prazo,
      statusTarefa: getStatusTarefaValue(values.statusTarefa),
      usuarioId: tarefa.usuarioId,
      projetoId: tarefa.projetoId,
    };

    console.log("Tarefa atualizada:", updatedTarefa);

    try {
      const response = await fetch(
        `http://localhost:5029/api/tarefa/${tarefa.id}`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(updatedTarefa),
        }
      );

      const data = await response.json();

      if (!response.ok) {
        console.error("Erro de resposta:", data);
        throw new Error("Falha ao atualizar a tarefa");
      }

      console.log("Tarefa atualizada com sucesso:", data);
      onSave(updatedTarefa);
      onClose();
    } catch (error) {
      console.error("Erro ao atualizar tarefa:", error);
    }
  };

  return (
    <div className={styles.modalOverlay}>
      <div className={styles.modalContent}>
        <h2>Editar Tarefa</h2>
        <Formik
          initialValues={initialValues}
          validationSchema={validationSchema}
          onSubmit={handleSave}
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
                <option value={0}>Não Iniciada</option>
                <option value={1}>Em Andamento</option>
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
