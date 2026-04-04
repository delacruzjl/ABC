import React from "react"
import { useNavigate } from "react-router-dom"
import { useChildren } from "../hooks/useChildren"
import { ChildList } from "../components/ChildList"

export const ChildrenPage: React.FC = () => {
  const { children, loading, error, removeChild } = useChildren()
  const navigate = useNavigate()

  if (loading) {
    return (
      <div className="flex justify-center py-12">
        <span className="text-cyan-400 text-lg">Loading children…</span>
      </div>
    )
  }

  if (error) {
    return (
      <div className="flex justify-center py-12">
        <span className="text-red-400 text-lg">Error: {error.message}</span>
      </div>
    )
  }

  return (
    <ChildList
      children={children}
      onAdd={() => navigate("/child/manage")}
      onEdit={(id) => navigate(`/child/edit/${id}`)}
      onDelete={(id) => removeChild(id)}
    />
  )
}
