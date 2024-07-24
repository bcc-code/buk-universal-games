import * as nb from '../locales/nb.json';
import * as en from '../locales/en.json';
import * as de from '../locales/de.json';
import * as es from '../locales/es.json';
import * as nl from '../locales/nl.json';
import * as pl from '../locales/pl.json';
import * as fr from '../locales/fr.json';
import * as ua from '../locales/ua.json';

const _check_en: typeof nb = en;
const _check_de: typeof nb = de;
const _check_es: typeof nb = es;
const _check_nl: typeof nb = nl;
const _check_pl: typeof nb = pl;
const _check_fr: typeof nb = fr;
const _check_ua: typeof nb = ua;

const _check_nb_en: typeof en = nb;
const _check_nb_de: typeof de = nb;
const _check_nb_es: typeof es = nb;
const _check_nb_nl: typeof nl = nb;
const _check_nb_pl: typeof pl = nb;
const _check_nb_fr: typeof fr = nb;
const _check_nb_ua: typeof ua = nb;

function removeUnusedWarning(...args: unknown[]): void {}

removeUnusedWarning(
  _check_en,
  _check_de,
  _check_es,
  _check_nl,
  _check_pl,
  _check_fr,
  _check_ua,
  _check_nb_en,
  _check_nb_de,
  _check_nb_es,
  _check_nb_nl,
  _check_nb_pl,
  _check_nb_fr,
  _check_nb_ua,
);
