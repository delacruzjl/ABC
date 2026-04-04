import React from "react"
import { useNavigate, useSearchParams } from "react-router-dom"
import { useConsequences } from "../hooks/useConsequences"
import { ConsequenceManager } from "../components/ConsequenceManager"
import type { Consequence } from "../types/consequence"

export const ConsequenceFormPage: React.FC = () => {
  const {
    consequences,
    createConsequence,
    updateConsequence,
    creating,
  } = useConsequences()
  const navigate = useNavigate()
  const [searchParams] = useSearchParams()
  const editId = searchParams.get("id")

  const existingConsequence = editId
    ? consequences.find((c) => c.id === editId) ?? null
    : null

  const handleSave = async (cons: Consequence) => {
    if (existingConsequence) {
      await updateConsequence(
        existingConsequence.id,
        cons.name,
        cons.description
      )
    } else {
      await createConsequence(cons.name, cons.description)
    }
    navigate("/consequences")
  }

  return (
    <ConsequenceManager
      consequence={existingConsequence}
      onSave={handleSave}
      onCancel={() => navigate("/consequences")}
      saving={creating}
    />
  )
}
