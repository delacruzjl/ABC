import React from "react"
import { render, screen, fireEvent } from "@testing-library/react"
import "@testing-library/jest-dom"
import { BehaviorList } from "../../components/BehaviorList"
import type { Behavior } from "../../types/behavior"

const mockBehaviors: Behavior[] = [
  { id: "1", name: "Hand flapping", description: "Repetitive hand movements" },
  { id: "2", name: "Screaming", description: "Loud vocal outburst" },
]

describe("BehaviorList", () => {
  const onAdd = jest.fn()
  const onEdit = jest.fn()
  const onDelete = jest.fn()

  beforeEach(() => {
    jest.clearAllMocks()
  })

  it("renders the heading", () => {
    render(
      <BehaviorList
        behaviors={[]}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    expect(screen.getByText("Behaviors")).toBeInTheDocument()
  })

  it("renders all behaviors", () => {
    render(
      <BehaviorList
        behaviors={mockBehaviors}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    expect(screen.getByText("Hand flapping")).toBeInTheDocument()
    expect(screen.getByText("Screaming")).toBeInTheDocument()
  })

  it("calls onAdd when Add button is clicked", () => {
    render(
      <BehaviorList
        behaviors={[]}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    fireEvent.click(screen.getByText("Add Behavior"))
    expect(onAdd).toHaveBeenCalledTimes(1)
  })

  it("calls onEdit with the correct id", () => {
    render(
      <BehaviorList
        behaviors={mockBehaviors}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    fireEvent.click(screen.getAllByText("Edit")[0])
    expect(onEdit).toHaveBeenCalledWith("1")
  })

  it("calls onDelete with the correct id", () => {
    render(
      <BehaviorList
        behaviors={mockBehaviors}
        onAdd={onAdd}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    )
    fireEvent.click(screen.getAllByText("Delete")[1])
    expect(onDelete).toHaveBeenCalledWith("2")
  })
})
