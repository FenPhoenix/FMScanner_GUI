/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1/GPL 2.0/LGPL 2.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Mozilla Universal charset detector code.
 *
 * The Initial Developer of the Original Code is
 * Netscape Communications Corporation.
 * Portions created by the Initial Developer are Copyright (C) 2001
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *          Shy Shalom <shooshX@gmail.com>
 *          Rudi Pettazzi <rudi.pettazzi@gmail.com> (C# port)
 * 
 * Alternatively, the contents of this file may be used under the terms of
 * either the GNU General Public License Version 2 or later (the "GPL"), or
 * the GNU Lesser General Public License Version 2.1 or later (the "LGPL"),
 * in which case the provisions of the GPL or the LGPL are applicable instead
 * of those above. If you wish to allow use of your version of this file only
 * under the terms of either the GPL or the LGPL, and not to allow others to
 * use your version of this file under the terms of the MPL, indicate your
 * decision by deleting the provisions above and replace them with the notice
 * and other provisions required by the GPL or the LGPL. If you do not delete
 * the provisions above, a recipient may use your version of this file under
 * the terms of any one of the MPL, the GPL or the LGPL.
 *
 * ***** END LICENSE BLOCK ***** */

using System;

namespace Ude.NetStandard;

internal abstract class GreekModel : SequenceModel
{
    // Model Table:
    // total sequences: 100%
    // first 512 sequences: 98.2851%
    // first 1024 sequences:1.7001%
    // rest  sequences:     0.0359%
    // negative sequences:  0.0148%
    /* Original uncompressed:
    private static readonly byte[] GREEK_LANG_MODEL = {
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,3,2,2,3,3,3,3,3,3,3,3,1,3,3,3,0,2,2,3,3,0,3,0,3,2,0,3,3,3,0,
        3,0,0,0,2,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,3,3,3,3,0,3,3,0,3,2,3,3,0,3,2,3,3,3,0,0,3,0,3,0,3,3,2,0,0,0,
        2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,
        0,2,3,2,2,3,3,3,3,3,3,3,3,0,3,3,3,3,0,2,3,3,0,3,3,3,3,2,3,3,3,0,
        2,0,0,0,2,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,2,3,3,2,3,3,3,3,3,3,3,3,3,3,3,3,0,2,1,3,3,3,3,2,3,3,2,3,3,2,0,
        0,0,0,0,2,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,3,3,3,0,3,3,3,3,3,3,0,3,3,0,3,3,3,3,3,3,3,3,3,3,0,3,2,3,3,0,
        2,0,1,0,2,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,
        0,3,3,3,3,3,2,3,0,0,0,0,3,3,0,3,1,3,3,3,0,3,3,0,3,3,3,3,0,0,0,0,
        2,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,3,3,3,3,0,3,0,3,3,3,3,3,0,3,2,2,2,3,0,2,3,3,3,3,3,2,3,3,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,3,3,3,3,3,2,2,2,3,3,3,3,0,3,1,3,3,3,3,2,3,3,3,3,3,3,3,2,2,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,3,3,3,3,2,0,3,0,0,0,3,3,2,3,3,3,3,3,0,0,3,2,3,0,2,3,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,0,3,3,3,3,0,0,3,3,0,2,3,0,3,0,3,3,3,0,0,3,0,3,0,2,2,3,3,0,0,
        0,0,1,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,3,3,3,3,2,0,3,2,3,3,3,3,0,3,3,3,3,3,0,3,3,2,3,2,3,3,2,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,3,2,3,2,3,3,3,3,3,3,0,2,3,2,3,2,2,2,3,2,3,3,2,3,0,2,2,2,3,0,
        2,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,3,0,0,0,3,3,3,2,3,3,0,0,3,0,3,0,0,0,3,2,0,3,0,3,0,0,2,0,2,0,
        0,0,0,0,2,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,3,3,3,0,3,3,3,3,3,3,0,3,3,0,3,0,0,0,3,3,0,3,3,3,0,0,1,2,3,0,
        3,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,3,3,3,3,2,0,0,3,2,2,3,3,0,3,3,3,3,3,2,1,3,0,3,2,3,3,2,1,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,3,3,0,2,3,3,3,3,3,3,0,0,3,0,3,0,0,0,3,3,0,3,2,3,0,0,3,3,3,0,
        3,0,0,0,2,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,3,3,3,0,3,3,3,3,3,3,0,0,3,0,3,0,0,0,3,2,0,3,2,3,0,0,3,2,3,0,
        2,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,3,1,2,2,3,3,3,3,3,3,0,2,3,0,3,0,0,0,3,3,0,3,0,2,0,0,2,3,1,0,
        2,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,0,3,3,3,3,0,3,0,3,3,2,3,0,3,3,3,3,3,3,0,3,3,3,0,2,3,0,0,3,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,0,3,3,3,0,0,3,0,0,0,3,3,0,3,0,2,3,3,0,0,3,0,3,0,3,3,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,3,0,0,0,3,3,3,3,3,3,0,0,3,0,2,0,0,0,3,3,0,3,0,3,0,0,2,0,2,0,
        0,0,0,0,1,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,3,3,3,3,3,0,3,0,2,0,3,2,0,3,2,3,2,3,0,0,3,2,3,2,3,3,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,3,0,0,2,3,3,3,3,3,0,0,0,3,0,2,1,0,0,3,2,2,2,0,3,0,0,2,2,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,0,3,3,3,2,0,3,0,3,0,3,3,0,2,1,2,3,3,0,0,3,0,3,0,3,3,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,2,3,3,3,0,3,3,3,3,3,3,0,2,3,0,3,0,0,0,2,1,0,2,2,3,0,0,2,2,2,0,
        0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,3,0,0,2,3,3,3,2,3,0,0,1,3,0,2,0,0,0,0,3,0,1,0,2,0,0,1,1,1,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,3,3,3,3,1,0,3,0,0,0,3,2,0,3,2,3,3,3,0,0,3,0,3,2,2,2,1,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,0,3,3,3,0,0,3,0,0,0,0,2,0,2,3,3,2,2,2,2,3,0,2,0,2,2,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,3,3,3,2,0,0,0,0,0,0,2,3,0,2,0,2,3,2,0,0,3,0,3,0,3,1,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,3,2,3,3,2,2,3,0,2,0,3,0,0,0,2,0,0,0,0,1,2,0,2,0,2,0,
        0,2,0,2,0,2,2,0,0,1,0,2,2,2,0,2,2,2,0,2,2,2,0,0,2,0,0,1,0,0,0,0,
        0,2,0,3,3,2,0,0,0,0,0,0,1,3,0,2,0,2,2,2,0,0,2,0,3,0,0,2,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,3,0,2,3,2,0,2,2,0,2,0,2,2,0,2,0,2,2,2,0,0,0,0,0,0,2,3,0,0,0,2,
        0,1,2,0,0,0,0,2,2,0,0,0,2,1,0,2,2,0,0,0,0,0,0,1,0,2,0,0,0,0,0,0,
        0,0,2,1,0,2,3,2,2,3,2,3,2,0,0,3,3,3,0,0,3,2,0,0,0,1,1,0,2,0,2,2,
        0,2,0,2,0,2,2,0,0,2,0,2,2,2,0,2,2,2,2,0,0,2,0,0,0,2,0,1,0,0,0,0,
        0,3,0,3,3,2,2,0,3,0,0,0,2,2,0,2,2,2,1,2,0,0,1,2,2,0,0,3,0,0,0,2,
        0,1,2,0,0,0,1,2,0,0,0,0,0,0,0,2,2,0,1,0,0,2,0,0,0,2,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,2,3,3,2,2,0,0,0,2,0,2,3,3,0,2,0,0,0,0,0,0,2,2,2,0,2,2,0,2,0,2,
        0,2,2,0,0,2,2,2,2,1,0,0,2,2,0,2,0,0,2,0,0,0,0,0,0,2,0,0,0,0,0,0,
        0,2,0,3,2,3,0,0,0,3,0,0,2,2,0,2,0,2,2,2,0,0,2,0,0,0,0,0,0,0,0,2,
        0,0,2,2,0,0,2,2,2,0,0,0,0,0,0,2,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,2,0,0,3,2,0,2,2,2,2,2,0,0,0,2,0,0,0,0,2,0,1,0,0,2,0,1,0,0,0,
        0,2,2,2,0,2,2,0,1,2,0,2,2,2,0,2,2,2,2,1,2,2,0,0,2,0,0,0,0,0,0,0,
        0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,
        0,2,0,2,0,2,2,0,0,0,0,1,2,1,0,0,2,2,0,0,2,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,3,2,3,0,0,2,0,0,0,2,2,0,2,0,0,0,1,0,0,2,0,2,0,2,2,0,0,0,0,
        0,0,2,0,0,0,0,2,2,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,
        0,2,2,3,2,2,0,0,0,0,0,0,1,3,0,2,0,2,2,0,0,0,1,0,2,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,2,0,2,0,3,2,0,2,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
        0,0,2,0,0,0,0,1,1,0,0,2,1,2,0,2,2,0,1,0,0,1,0,0,0,2,0,0,0,0,0,0,
        0,3,0,2,2,2,0,0,2,0,0,0,2,0,0,0,2,3,0,2,0,0,0,0,0,0,2,2,0,0,0,2,
        0,1,2,0,0,0,1,2,2,1,0,0,0,2,0,0,2,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,2,1,2,0,2,2,0,2,0,0,2,0,0,0,0,1,2,1,0,2,1,0,0,0,0,0,0,0,0,0,0,
        0,0,2,0,0,0,3,1,2,2,0,2,0,0,0,0,2,0,0,0,2,0,0,3,0,0,0,0,2,2,2,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,2,1,0,2,0,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,1,0,0,0,0,0,0,2,
        0,2,2,0,0,2,2,2,2,2,0,1,2,0,0,0,2,2,0,1,0,2,0,0,2,2,0,0,0,0,0,0,
        0,0,0,0,1,0,0,0,0,0,0,0,3,0,0,2,0,0,0,0,0,0,0,0,2,0,2,0,0,0,0,2,
        0,1,2,0,0,0,0,2,2,1,0,1,0,1,0,2,2,2,1,0,0,0,0,0,0,1,0,0,0,0,0,0,
        0,2,0,1,2,0,0,0,0,0,0,0,0,0,0,2,0,0,2,2,0,0,0,0,1,0,0,0,0,0,0,2,
        0,2,2,0,0,0,0,2,2,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,2,0,0,2,0,0,0,
        0,2,2,2,2,0,0,0,3,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,2,0,0,0,0,0,0,1,
        0,0,2,0,0,0,0,1,2,0,0,0,0,0,0,2,2,1,1,0,0,0,0,0,0,1,0,0,0,0,0,0,
        0,2,0,2,2,2,0,0,2,0,0,0,0,0,0,0,2,2,2,0,0,0,2,0,0,0,0,0,0,0,0,2,
        0,0,1,0,0,0,0,2,1,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,
        0,3,0,2,0,0,0,0,0,0,0,0,2,0,0,0,0,0,2,0,0,0,0,0,0,0,2,0,0,0,0,2,
        0,0,2,0,0,0,0,2,2,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,2,0,2,2,1,0,0,0,0,0,0,2,0,0,2,0,2,2,2,0,0,0,0,0,0,2,0,0,0,0,2,
        0,0,2,0,0,2,0,2,2,0,0,0,0,2,0,2,0,0,0,0,0,2,0,0,0,2,0,0,0,0,0,0,
        0,0,3,0,0,0,2,2,0,2,2,0,0,0,0,0,2,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,2,0,0,0,0,0,
        0,2,2,2,2,2,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,1,
        0,0,0,0,0,0,0,2,1,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,2,2,0,0,0,0,0,2,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,
        0,2,0,0,0,2,0,0,0,0,0,1,0,0,0,0,2,2,0,0,0,1,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,2,0,0,0,
        0,2,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,2,0,2,0,0,0,
        0,0,0,0,0,0,0,0,2,1,0,0,0,0,0,0,2,0,0,0,1,2,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
        0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
    };
    */
    private static readonly byte[] GREEK_LANG_MODEL_COMPRESSED =
    {
        197, 86, 91, 178, 235, 48, 8, 67, 242, 254, 247, 124, 194, 211, 216, 105, 82, 119, 238, 199, 117, 167,
        109, 30, 6, 9, 16, 36, 34, 255, 186, 6, 57, 98, 225, 250, 138, 158, 202, 245, 161, 12, 59, 16, 161, 237,
        227, 131, 249, 176, 109, 106, 16, 63, 151, 137, 57, 24, 124, 182, 154, 151, 217, 240, 205, 153, 48, 14,
        204, 213, 244, 241, 228, 201, 55, 214, 18, 34, 140, 233, 12, 14, 248, 203, 152, 81, 76, 71, 22, 16, 5,
        159, 236, 177, 198, 207, 225, 135, 226, 41, 204, 72, 194, 234, 1, 185, 217, 203, 200, 127, 94, 217, 16,
        134, 211, 241, 102, 216, 237, 205, 108, 36, 126, 37, 132, 175, 200, 157, 191, 213, 57, 13, 85, 19, 74,
        226, 0, 93, 36, 67, 181, 194, 249, 153, 215, 159, 193, 63, 83, 245, 166, 31, 186, 112, 42, 11, 215, 89,
        85, 239, 43, 127, 102, 188, 170, 37, 203, 132, 21, 95, 60, 149, 181, 241, 201, 155, 71, 238, 217, 118,
        189, 91, 58, 134, 90, 28, 232, 231, 96, 125, 173, 127, 215, 159, 72, 200, 231, 202, 28, 253, 252, 213,
        75, 228, 207, 219, 56, 206, 224, 226, 37, 62, 155, 236, 14, 36, 243, 39, 19, 159, 86, 210, 222, 255, 15,
        114, 232, 252, 91, 254, 152, 42, 154, 27, 159, 240, 193, 89, 191, 194, 87, 67, 14, 124, 175, 95, 232,
        207, 69, 51, 19, 233, 250, 61, 144, 112, 41, 54, 113, 231, 252, 250, 110, 156, 113, 205, 248, 153, 126,
        154, 126, 240, 202, 63, 7, 0, 35, 111, 145, 185, 179, 238, 119, 252, 108, 91, 245, 2, 147, 130, 53, 244,
        73, 251, 123, 252, 204, 136, 137, 95, 227, 103, 171, 127, 212, 239, 226, 160, 173, 167, 13, 216, 55, 190,
        243, 87, 3, 132, 94, 134, 15, 93, 224, 64, 192, 134, 140, 166, 187, 224, 127, 129, 159, 201, 127, 214,
        95, 11, 102, 163, 212, 116, 123, 148, 61, 199, 143, 157, 110, 102, 205, 168, 159, 35, 248, 112, 98, 184,
        166, 1, 137, 76, 129, 174, 31, 6, 21, 88, 58, 243, 107, 233, 113, 80, 41, 124, 56, 109, 187, 59, 78, 7,
        150, 77, 77, 69, 168, 159, 25, 141, 250, 128, 157, 218, 69, 171, 107, 64, 77, 239, 122, 117, 216, 208,
        165, 68, 46, 245, 30, 80, 30, 197, 41, 5, 119, 230, 67, 17, 129, 111, 207, 169, 225, 32, 90, 53, 13, 141,
        20, 105, 248, 72, 52, 154, 217, 215, 135, 234, 15, 139, 241, 152, 180, 218, 167, 87, 167, 58, 185, 187,
        152, 24, 5, 241, 45, 105, 239, 179, 46, 250, 109, 86, 167, 118, 133, 131, 105, 181, 49, 183, 217, 109,
        16, 117, 51, 130, 68, 163, 130, 153, 63, 44, 8, 182, 208, 189, 237, 23, 82, 202, 136, 24, 118, 2, 198,
        223, 176, 125, 96, 161, 27, 37, 163, 45, 234, 22, 63, 235, 53, 3, 213, 54, 56, 47, 15, 125, 240, 85, 226,
        215, 187, 200, 110, 208, 3, 48, 234, 143, 134, 63, 164, 73, 42, 122, 112, 186, 42, 253, 48, 108, 216, 60,
        175, 89, 216, 3, 76, 126, 96, 47, 188, 38, 113, 29, 44, 54, 241, 17, 185, 75, 30, 62, 78, 78, 70, 136,
        250, 155, 250, 46, 151, 200, 163, 212, 95, 108, 98, 188, 144, 118, 215, 73, 167, 247, 124, 209, 201, 254,
        133, 192, 103, 8, 86, 163, 21, 188, 60, 47, 248, 15, 245, 143, 59, 118, 111, 180, 235, 237, 175, 234, 87,
        69, 193, 13, 127, 233, 152, 214, 5, 61, 19, 43, 235, 158, 255, 177, 108, 110, 182, 89, 142, 198, 31, 247,
        178, 27, 126, 5, 187, 204, 191, 105, 95, 89, 104, 16, 133, 90, 147, 235, 67, 248, 167, 235, 30, 214, 253,
        238, 198, 186, 141, 170, 123, 113, 204, 10, 205, 180, 133, 248, 113, 173, 236, 247, 129, 194, 121, 45,
        218, 251, 153, 223, 89, 228, 168, 25, 193, 221, 98, 119, 96, 222, 79, 222, 4, 222, 192, 110, 254, 118,
        224, 158, 32, 252, 88, 189, 255, 185, 254, 0
    };
    private static readonly byte[] GREEK_LANG_MODEL;

    static GreekModel()
    {
        GREEK_LANG_MODEL = Utils.Decompress(GREEK_LANG_MODEL_COMPRESSED, 4096);
        GREEK_LANG_MODEL_COMPRESSED = Array.Empty<byte>();
    }

    protected GreekModel(byte[] charToOrderMap, Charset name)
        : base(charToOrderMap, GREEK_LANG_MODEL, 0.982851f, name)
    {
    }
}

internal sealed class Latin7Model : GreekModel
{
    /****************************************************************
    255: Control characters that usually does not exist in any text
    254: Carriage/Return
    253: symbol (punctuation) that does not belong to word
    252: 0 - 9
    *****************************************************************/
    //Character Mapping Table:
    private static readonly byte[] LATIN7_CHAR_TO_ORDER_MAP = {
        255,255,255,255,255,255,255,255,255,255,254,255,255,254,255,255,  //00
        255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,  //10
        +253,253,253,253,253,253,253,253,253,253,253,253,253,253,253,253,  //20
        252,252,252,252,252,252,252,252,252,252,253,253,253,253,253,253,  //30
        253, 82,100,104, 94, 98,101,116,102,111,187,117, 92, 88,113, 85,  //40
        79,118,105, 83, 67,114,119, 95, 99,109,188,253,253,253,253,253,  //50
        253, 72, 70, 80, 81, 60, 96, 93, 89, 68,120, 97, 77, 86, 69, 55,  //60
        78,115, 65, 66, 58, 76,106,103, 87,107,112,253,253,253,253,253,  //70
        255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,  //80
        255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,  //90
        +253,233, 90,253,253,253,253,253,253,253,253,253,253, 74,253,253,  //a0
        253,253,253,253,247,248, 61, 36, 46, 71, 73,253, 54,253,108,123,  //b0
        110, 31, 51, 43, 41, 34, 91, 40, 52, 47, 44, 53, 38, 49, 59, 39,  //c0
        35, 48,250, 37, 33, 45, 56, 50, 84, 57,120,121, 17, 18, 22, 15,  //d0
        124,  1, 29, 20, 21,  3, 32, 13, 25,  5, 11, 16, 10,  6, 30,  4,  //e0
        9,  8, 14,  7,  2, 12, 28, 23, 42, 24, 64, 75, 19, 26, 27,253   //f0
    };

    public Latin7Model() : base(LATIN7_CHAR_TO_ORDER_MAP, Charset.ISO_8859_7)
    {
    }
}

internal sealed class Win1253Model : GreekModel
{
    private static readonly byte[] WIN1253__CHAR_TO_ORDER_MAP = {
        255,255,255,255,255,255,255,255,255,255,254,255,255,254,255,255,  //00
        255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,  //10
        +253,253,253,253,253,253,253,253,253,253,253,253,253,253,253,253,  //20
        252,252,252,252,252,252,252,252,252,252,253,253,253,253,253,253,  //30
        253, 82,100,104, 94, 98,101,116,102,111,187,117, 92, 88,113, 85,  //40
        79,118,105, 83, 67,114,119, 95, 99,109,188,253,253,253,253,253,  //50
        253, 72, 70, 80, 81, 60, 96, 93, 89, 68,120, 97, 77, 86, 69, 55,  //60
        78,115, 65, 66, 58, 76,106,103, 87,107,112,253,253,253,253,253,  //70
        255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,  //80
        255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,  //90
        +253,233, 61,253,253,253,253,253,253,253,253,253,253, 74,253,253,  //a0
        253,253,253,253,247,253,253, 36, 46, 71, 73,253, 54,253,108,123,  //b0
        110, 31, 51, 43, 41, 34, 91, 40, 52, 47, 44, 53, 38, 49, 59, 39,  //c0
        35, 48,250, 37, 33, 45, 56, 50, 84, 57,120,121, 17, 18, 22, 15,  //d0
        124,  1, 29, 20, 21,  3, 32, 13, 25,  5, 11, 16, 10,  6, 30,  4,  //e0
        9,  8, 14,  7,  2, 12, 28, 23, 42, 24, 64, 75, 19, 26, 27,253   //f0
    };

    public Win1253Model() : base(WIN1253__CHAR_TO_ORDER_MAP, Charset.Windows1253)
    {
    }
}
