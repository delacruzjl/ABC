import React from "react"
import { useNavigate, useSearchParams } from "react-router-dom"
import { useAntecedents } from "../hooks/useAntecedents"
import { AntecedentManager } from "../components/AntecedentManager"
import type { Antecedent } from "../types/antecedent"

export const AntecedentFormPage: React.FC = () => {
  const {
    antecedents,
    createAntecedent,
    updateAntecedent,
    creating,
  } = useAntecedents()
  const navigate = useNavigate()
  const [searchParams] = useSearchParams()
  const editId = searchParams.get("id")

  const existingAntecedent = editId
    ? antecedents.find((a) => a.id === editId) ?? null
    : null

  const handleSave = async (ant: Antecedent) => {
    if (existingAntecedent) {
      await updateAntecedent(existingAntecedent.id, ant.name, ant.description)
    } else {
      await createAntecedent(ant.name, ant.description)
    }
    navigate("/antecedents")
  }

  return (
    <AntecedentManager
      antecedent={existingAntecedent}
      onSave={handleSave}
      onCancel={() => navigate("/antecedents")}
      saving={creating}
    />
  )
}
