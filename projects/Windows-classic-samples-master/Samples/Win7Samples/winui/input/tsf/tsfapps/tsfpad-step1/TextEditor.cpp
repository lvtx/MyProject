
#include "private.h"
#include "TextEditor.h"

extern ITfThreadMgr *g_pThreadMgr;
extern TfClientId g_TfClientId;

//----------------------------------------------------------------
//
//
//
//----------------------------------------------------------------

void CTextEditor::MoveSelection(UINT nSelStart, UINT nSelEnd)
{
    UINT nTextLength = GetTextLength();
    if (nSelStart >= nTextLength)
        nSelStart = nTextLength;

    if (nSelEnd >= nTextLength)
        nSelEnd = nTextLength;

    _nSelStart = nSelStart;
    _nSelEnd = nSelEnd;

    _pTextStore->OnSelectionChange();
}

//----------------------------------------------------------------
//
//
//
//----------------------------------------------------------------

void CTextEditor::MoveSelectionNext()
{
    UINT nTextLength = GetTextLength();
    if (_nSelEnd < nTextLength)
       _nSelEnd++;

    _nSelStart = _nSelEnd;
    _pTextStore->OnSelectionChange();
}

//----------------------------------------------------------------
//
//
//
//----------------------------------------------------------------

void CTextEditor::MoveSelectionPrev()
{
    if (_nSelStart > 0)
        _nSelStart--;

    _nSelEnd = _nSelStart;
    _pTextStore->OnSelectionChange();
}

//----------------------------------------------------------------
//
//
//
//----------------------------------------------------------------

BOOL CTextEditor::MoveSelectionAtPoint(POINT pt)
{
    BOOL bRet = FALSE;
    UINT nSel = _layout.CharPosFromPoint(pt);
    if (nSel != (UINT)-1)
    {
        MoveSelection(nSel, nSel);
        bRet = TRUE;
    }
    return bRet;
}

//----------------------------------------------------------------
//
//
//
//----------------------------------------------------------------

BOOL CTextEditor::MoveSelectionUpDown(BOOL bUp)
{
    RECT rc;
    if (!_layout.RectFromCharPos(_nSelStart, &rc))
        return FALSE;

    POINT pt;
    pt.x = rc.left;
    if (bUp)
    {
        pt.y = rc.top - ((rc.bottom - rc.top) / 2);
        if (pt.y < 0)
            return FALSE;
    }
    else
    {
        pt.y = rc.bottom + ((rc.bottom - rc.top) / 2);
    }

    return MoveSelectionAtPoint(pt);
}

//----------------------------------------------------------------
//
//
//
//----------------------------------------------------------------

BOOL CTextEditor::MoveSelectionToLineFirstEnd(BOOL bFirst)
{
    BOOL bRet = FALSE;
    UINT nSel;

    if (bFirst)
    {
        nSel = _layout.FineFirstEndCharPosInLine(_nSelStart, TRUE);
    }
    else
    {
        nSel = _layout.FineFirstEndCharPosInLine(_nSelEnd, FALSE);
    }

    if (nSel != (UINT)-1)
    {
        MoveSelection(nSel, nSel);
        bRet = TRUE;
    }
    return bRet;
}

//----------------------------------------------------------------
//
//
//
//----------------------------------------------------------------

BOOL CTextEditor::InsertAtSelection(LPCWSTR psz)
{
    LONG lOldSelEnd = _nSelEnd;
    if (!RemoveText(_nSelStart, _nSelEnd - _nSelStart))
        return FALSE;

    if (!InsertText(_nSelStart, psz, lstrlen(psz)))
        return FALSE;

    _nSelStart += lstrlen(psz);
    _nSelEnd = _nSelStart;

    _pTextStore->OnTextChange(_nSelStart, lOldSelEnd, _nSelEnd);
    _pTextStore->OnSelectionChange();
    return TRUE;
}

//----------------------------------------------------------------
//
//
//
//----------------------------------------------------------------

BOOL CTextEditor::DeleteAtSelection(BOOL fBack)
{
    if (!fBack && (_nSelEnd < GetTextLength()))
    {
        if (!RemoveText(_nSelEnd, 1))
            return FALSE;

        _pTextStore->OnTextChange(_nSelEnd, _nSelEnd + 1, _nSelEnd);
    }

    if (fBack && (_nSelStart > 0))
    {
        if (!RemoveText(_nSelStart - 1, 1))
            return FALSE;

        _nSelStart--;
        _nSelEnd = _nSelStart;

        _pTextStore->OnTextChange(_nSelStart, _nSelStart + 1, _nSelStart);
        _pTextStore->OnSelectionChange();
    }

    return TRUE;
}

//----------------------------------------------------------------
//
//
//
//----------------------------------------------------------------

BOOL CTextEditor::DeleteSelection()
{
    ULONG nSelOldEnd = _nSelEnd;
    RemoveText(_nSelStart, _nSelEnd - _nSelStart);

    _nSelEnd = _nSelStart;

    _pTextStore->OnTextChange(_nSelStart, nSelOldEnd, _nSelStart);
    _pTextStore->OnSelectionChange();

    return TRUE;
}

//----------------------------------------------------------------
//
//
//
//----------------------------------------------------------------

void CTextEditor::Render(HDC hdc, const LOGFONT *plf)
{
    HFONT hFont = CreateFontIndirect(plf);

    if (hFont)
    {
        HFONT hFontOrg = (HFONT)SelectObject(hdc, hFont);

        _layout.Layout(hdc, GetTextBuffer(), GetTextLength());
        _layout.Render(hdc, GetTextBuffer(), GetTextLength(), _nSelStart, _nSelEnd);

        SelectObject(hdc, hFontOrg);
        DeleteObject(hFont);
    }
}

//----------------------------------------------------------------
//
//
//
//----------------------------------------------------------------

BOOL CTextEditor::InitTSF()
{

    _pTextStore = new CTextStore(this);
    if (!_pTextStore)
    {
        return FALSE;
    }

    if (FAILED(g_pThreadMgr->CreateDocumentMgr(&_pDocumentMgr)))
    {
        return FALSE;
    }

    if (FAILED(_pDocumentMgr->CreateContext(g_TfClientId, 0, _pTextStore, &_pInputContext, &_ecTextStore)))
    {
        return FALSE;
    }

    if (FAILED(_pDocumentMgr->Push(_pInputContext)))
    {
        return FALSE;
    }

    ITfDocumentMgr *pDocumentMgrPrev;
    g_pThreadMgr->AssociateFocus(_hwnd, _pDocumentMgr, &pDocumentMgrPrev);
    if (pDocumentMgrPrev)
        pDocumentMgrPrev->Release();
    return TRUE;
}

//----------------------------------------------------------------
//
//
//
//----------------------------------------------------------------

BOOL CTextEditor::UninitTSF()
{
    if (_pDocumentMgr)
    {
        _pDocumentMgr->Pop(TF_POPF_ALL);
    }

    if (_pInputContext)
    {
        _pInputContext->Release();
        _pInputContext = NULL;
    }

    if (_pDocumentMgr)
    {
        _pDocumentMgr->Release();
        _pDocumentMgr = NULL;
    }

    if (_pTextStore)
    {
        _pTextStore->Release();
        _pTextStore = NULL;
    }

    return TRUE;
}


//----------------------------------------------------------------
//
//
//
//----------------------------------------------------------------

void CTextEditor::SetFocusDocumentMgr()
{
    if (_pDocumentMgr)
    {
        // g_pThreadMgr->SetFocus(_pDocumentMgr);
    }
}

