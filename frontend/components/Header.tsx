"use client";
import { useState } from "react";
import Link from "next/link";
import styles from "@/components/styles/header.module.css";

export default function Header() {
  const [menuOpen, setMenuOpen] = useState(false);

  return (
    <>
      <button
        type="button"
        className={`${styles.hamburger} ${menuOpen ? styles.hamburgerOpen : ""}`}
        aria-label="Toggle navigation"
        aria-expanded={menuOpen}
        onClick={() => setMenuOpen((v) => !v)}
      >
        <span className={styles.topBun}></span>
        <span className={styles.meat}></span>
        <span className={styles.bottomBun}></span>
      </button>

      <div className={`${styles.nav} ${menuOpen ? styles.navOpen : ""}`}>
        <nav className={styles.navContainer}>
          <ul className={styles.navLinks}>
            <li><Link href="/">Rapid Crew</Link></li>
            <li><Link href="/events">Events</Link></li>
            <li><Link href="/merch">Merch</Link></li>
            <li><Link href="/login">Login</Link></li>
            <li><Link href="/register">Register</Link></li>
          </ul>
        </nav>
      </div>
    </>
  );
}