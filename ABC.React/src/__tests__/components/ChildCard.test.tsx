import React from "react"
import { render, screen, fireEvent } from "@testing-library/react"
import "@testing-library/jest-dom"
import { MemoryRouter } from "react-router-dom"
import { ChildCard } from "../../components/ChildCard"
import type { Child } from "../../types/child"

const mockChild: Child = {
  id: "child-1",
  firstName: "Jane",
  lastName: "Doe",
  birthYear: 2018,
  userId: "user-1",
  conditions: [],
  observationCount: 3,
}

function renderCard(props: Partial<React.ComponentProps<typeof ChildCard>> = {}) {
  return render(
    <MemoryRouter>
      <ChildCard
        child={mockChild}
        onDelete={jest.fn()}
        {...props}
      />
    </MemoryRouter>
  )
}

describe("ChildCard", () => {
  it("shows Default badge when isDefault is true", () => {
    renderCard({ isDefault: true, onSetDefault: jest.fn() })
    expect(screen.getByText("Default")).toBeInTheDocument()
  })

  it("does not show Default badge when isDefault is false", () => {
    renderCard({ isDefault: false, onSetDefault: jest.fn() })
    expect(screen.queryByText("Default")).not.toBeInTheDocument()
  })

  it("shows filled star when isDefault", () => {
    renderCard({ isDefault: true, onSetDefault: jest.fn() })
    expect(screen.getByText("★")).toBeInTheDocument()
  })

  it("shows empty star when not default", () => {
    renderCard({ isDefault: false, onSetDefault: jest.fn() })
    expect(screen.getByText("☆")).toBeInTheDocument()
  })

  it("calls onSetDefault with child id when star is clicked (setting default)", () => {
    const onSetDefault = jest.fn()
    renderCard({ isDefault: false, onSetDefault })
    fireEvent.click(screen.getByText("☆"))
    expect(onSetDefault).toHaveBeenCalledWith("child-1")
  })

  it("calls onSetDefault with null when star is clicked (clearing default)", () => {
    const onSetDefault = jest.fn()
    renderCard({ isDefault: true, onSetDefault })
    fireEvent.click(screen.getByText("★"))
    expect(onSetDefault).toHaveBeenCalledWith(null)
  })

  it("does not render star button when onSetDefault is not provided", () => {
    renderCard({ isDefault: false })
    expect(screen.queryByText("☆")).not.toBeInTheDocument()
    expect(screen.queryByText("★")).not.toBeInTheDocument()
  })

  it("renders Observe button", () => {
    renderCard()
    expect(screen.getByText("Observe")).toBeInTheDocument()
  })
})
