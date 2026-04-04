import React from "react"
import { useNavigate, useSearchParams } from "react-router-dom"
import { useBehaviors } from "../hooks/useBehaviors"
import { BehaviorManager } from "../components/BehaviorManager"
import type { Behavior } from "../types/behavior"

export const BehaviorFormPage: React.FC = () => {
  const {
    behaviors,
    createBehavior,
    updateBehavior,
    creating,
  } = useBehaviors()
  const navigate = useNavigate()
  const [searchParams] = useSearchParams()
  const editId = searchParams.get("id")

  const existingBehavior = editId
    ? behaviors.find((b) => b.id === editId) ?? null
    : null

  const handleSave = async (beh: Behavior) => {
    if (existingBehavior) {
      await updateBehavior(existingBehavior.id, beh.name, beh.description)
    } else {
      await createBehavior(beh.name, beh.description)
    }
    navigate("/behaviors")
  }

  return (
    <BehaviorManager
      behavior={existingBehavior}
      onSave={handleSave}
      onCancel={() => navigate("/behaviors")}
      saving={creating}
    />
  )
}
